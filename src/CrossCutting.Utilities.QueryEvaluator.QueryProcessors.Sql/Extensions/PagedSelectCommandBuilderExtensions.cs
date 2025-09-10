namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;

internal static class PagedSelectCommandBuilderExtensions
{
    internal static Result<PagedSelectCommandBuilder> Select(this PagedSelectCommandBuilder instance,
                                                             IQueryContext context,
                                                             IPagedDatabaseEntityRetrieverSettings settings,
                                                             IQueryFieldInfo fieldInfo,
                                                             ISqlExpressionProvider sqlExpressionProvider,
                                                             ParameterBag parameterBag)
    {
        var fieldSelectionQuery = context.Query as IFieldSelectionQuery;

        return fieldSelectionQuery is null || fieldSelectionQuery.GetAllFields
                ? instance.AppendSelectFieldsForAllFields(settings, fieldInfo)
                : instance.AppendSelectFieldsForSpecifiedFields(context, fieldSelectionQuery, fieldInfo, sqlExpressionProvider, parameterBag);
    }

    private static Result<PagedSelectCommandBuilder> AppendSelectFieldsForAllFields(this PagedSelectCommandBuilder instance,
                                                                                    IPagedDatabaseEntityRetrieverSettings settings,
                                                                                    IQueryFieldInfo fieldInfo)
    {
        var allFields = fieldInfo.GetAllFields();
        return Result.Success(allFields.Any()
            ? instance.Select(string.Join(", ", allFields
                .Select(fieldInfo.GetDatabaseFieldName)
                .Where(x => !string.IsNullOrEmpty(x))))
            : instance.Select(settings.Fields.WhenNullOrWhitespace("*")));
    }

    private static Result<PagedSelectCommandBuilder> AppendSelectFieldsForSpecifiedFields(this PagedSelectCommandBuilder instance,
                                                                                          IQueryContext context,
                                                                                          IFieldSelectionQuery fieldSelectionQuery,
                                                                                          IQueryFieldInfo fieldInfo,
                                                                                          ISqlExpressionProvider sqlExpressionProvider,
                                                                                          ParameterBag parameterBag)
    {
        foreach (var expression in fieldSelectionQuery.FieldNames.Select((x, index) => new { Item = x, Index = index }))
        {
            if (expression.Index > 0)
            {
                instance.Select(", ");
            }

            var result = sqlExpressionProvider.GetSqlExpression(fieldSelectionQuery.WithContext(context.Context), new PropertyNameExpressionBuilder(expression.Item).Build(), fieldInfo, parameterBag).EnsureValue();
            if (!result.IsSuccessful())
            {
                return Result.FromExistingResult<PagedSelectCommandBuilder>(result);
            }

            instance.Select(result.Value!);
        }

        return Result.Success(instance);
    }

    internal static Result<PagedSelectCommandBuilder> Distinct(this PagedSelectCommandBuilder instance, IQueryContext context)
    {
        var fieldSelectionQuery = context.Query as IFieldSelectionQuery;

        return Result.Success(instance.DistinctValues(fieldSelectionQuery?.Distinct == true));
    }

    internal static Result<PagedSelectCommandBuilder> Top(this PagedSelectCommandBuilder instance,
                                                  IQueryContext context,
                                                  IPagedDatabaseEntityRetrieverSettings settings,
                                                  int? customLimit)
    {
        var limit = context.Query.Limit.IfNotGreaterThan(settings.OverridePageSize, customLimit);

        return Result.Success(limit > 0
            ? instance.WithTop(limit)
            : instance);
    }

    internal static Result<PagedSelectCommandBuilder> Offset(this PagedSelectCommandBuilder instance,
                                                             IQueryContext context,
                                                             int? customPageSize)
    {
        var offset = context.Query.Offset.GetValueOrDefault(customPageSize.GetValueOrDefault());

        return Result.Success(offset > 0
            ? instance.Skip(offset)
            : instance);
    }

    internal static Result<PagedSelectCommandBuilder> From(this PagedSelectCommandBuilder instance,
                                                           IQueryContext context,
                                                           IPagedDatabaseEntityRetrieverSettings settings)
        => Result.Success(instance.From(context.Query.GetTableName(settings.TableName)));

    internal static Result<PagedSelectCommandBuilder> Where(this PagedSelectCommandBuilder instance,
                                                            IQueryContext context,
                                                            IPagedDatabaseEntityRetrieverSettings settings,
                                                            IQueryFieldInfo fieldInfo,
                                                            ISqlExpressionProvider sqlExpressionProvider,
                                                            ISqlConditionExpressionProvider provider,
                                                            ParameterBag parameterBag)
    {
        if (context.Query.Conditions.Count == 0 && string.IsNullOrEmpty(settings.DefaultWhere))
        {
            return Result.Success(instance);
        }

        if (!string.IsNullOrEmpty(settings.DefaultWhere))
        {
            instance.Where(settings.DefaultWhere);
        }

        foreach (var queryCondition in context.Query.Conditions)
        {
            var result = provider.GetConditionExpression(
                context,
                queryCondition,
                fieldInfo,
                sqlExpressionProvider,
                parameterBag
            );

            if (!result.IsSuccessful())
            {
                return Result.FromExistingResult<PagedSelectCommandBuilder>(result);
            }

            result.OnSuccess(value => (queryCondition.Combination ?? Combination.And) == Combination.And
                ? instance.And(value)
                : instance.Or(value));
        }

        return Result.Success(instance);
    }

    internal static Result<PagedSelectCommandBuilder> OrderBy(this PagedSelectCommandBuilder instance,
                                                              IQueryContext context,
                                                              IPagedDatabaseEntityRetrieverSettings settings,
                                                              IQueryFieldInfo fieldInfo,
                                                              ISqlExpressionProvider sqlExpressionProvider,
                                                              ParameterBag parameterBag)
    {
        if (context.Query.SortOrders.Count > 0 || !string.IsNullOrEmpty(settings.DefaultOrderBy))
        {
            return instance.AppendOrderBy(context, settings, fieldInfo, sqlExpressionProvider, parameterBag);
        }
        else
        {
            return Result.Success(instance);
        }
    }

    private static Result<PagedSelectCommandBuilder> AppendOrderBy(this PagedSelectCommandBuilder instance,
                                                                   IQueryContext context,
                                                                   IPagedDatabaseEntityRetrieverSettings settings,
                                                                   IQueryFieldInfo fieldInfo,
                                                                   ISqlExpressionProvider sqlExpressionProvider,
                                                                   ParameterBag parameterBag)
    {
        foreach (var querySortOrder in context.Query.SortOrders.Select((x, index) => new { Item = x, Index = index }))
        {
            if (querySortOrder.Index > 0)
            {
                instance.OrderBy(", ");
            }

            instance.OrderBy($"{sqlExpressionProvider.GetSqlExpression(context, querySortOrder.Item.Expression, fieldInfo, parameterBag).GetValueOrThrow()} {querySortOrder.Item.ToSql()}");
        }

        if (context.Query.SortOrders.Count == 0 && !string.IsNullOrEmpty(settings.DefaultOrderBy))
        {
            instance.OrderBy(settings.DefaultOrderBy);
        }

        return Result.Success(instance);
    }

    internal static Result<PagedSelectCommandBuilder> WithParameters(this PagedSelectCommandBuilder instance,
                                                                     IQueryContext context,
                                                                     ParameterBag parameterBag)
    {
        var parameterizedQuery = context.Query as IParameterizedQuery;

        if (parameterizedQuery is not null)
        {
            foreach (var parameter in parameterizedQuery.Parameters)
            {
                instance.AppendParameter(parameter.Name, parameter.Value!);
            }
        }

        foreach (var parameter in parameterBag.Parameters)
        {
            instance.AppendParameter(parameter.Key, parameter.Value!);
        }

        return Result.Success(instance);
    }
}
