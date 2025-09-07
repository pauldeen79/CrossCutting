namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;

internal static class PagedSelectCommandBuilderExtensions
{
    internal static PagedSelectCommandBuilder Select(this PagedSelectCommandBuilder instance,
                                                     IQueryContext context,
                                                     IPagedDatabaseEntityRetrieverSettings settings,
                                                     IQueryFieldInfo fieldInfo,
                                                     IFieldSelectionQuery? fieldSelectionQuery,
                                                     ISqlExpressionProvider sqlExpressionProvider,
                                                     ParameterBag parameterBag)
        => fieldSelectionQuery is null || fieldSelectionQuery.GetAllFields
            ? instance.AppendSelectFieldsForAllFields(settings, fieldInfo)
            : instance.AppendSelectFieldsForSpecifiedFields(context, fieldSelectionQuery, fieldInfo, sqlExpressionProvider, parameterBag);

    private static PagedSelectCommandBuilder AppendSelectFieldsForAllFields(this PagedSelectCommandBuilder instance,
                                                                            IPagedDatabaseEntityRetrieverSettings settings,
                                                                            IQueryFieldInfo fieldInfo)
    {
        var allFields = fieldInfo.GetAllFields();
        return allFields.Any()
            ? instance.Select(string.Join(", ", allFields
                .Select(fieldInfo.GetDatabaseFieldName)
                .Where(x => !string.IsNullOrEmpty(x))))
            : instance.Select(settings.Fields.WhenNullOrWhitespace("*"));
    }

    private static PagedSelectCommandBuilder AppendSelectFieldsForSpecifiedFields(this PagedSelectCommandBuilder instance,
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

            instance.Select(sqlExpressionProvider.GetSqlExpression(fieldSelectionQuery.WithContext(context.Context), new PropertyNameExpressionBuilder(expression.Item).Build(), fieldInfo, parameterBag).GetValueOrThrow());
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder Distinct(this PagedSelectCommandBuilder instance,
                                                       IFieldSelectionQuery? fieldSelectionQuery)
        => instance.DistinctValues(fieldSelectionQuery?.Distinct == true);

    internal static PagedSelectCommandBuilder Top(this PagedSelectCommandBuilder instance,
                                                  IQueryContext context,
                                                  IPagedDatabaseEntityRetrieverSettings settings,
                                                  int? customLimit)
    {
        var limit = context.Query.Limit.IfNotGreaterThan(settings.OverridePageSize, customLimit);

        return limit > 0
            ? instance.WithTop(limit)
            : instance;
    }

    internal static PagedSelectCommandBuilder Offset(this PagedSelectCommandBuilder instance,
                                                     IQueryContext context,
                                                     int? customPageSize)
    {
        var offset = context.Query.Offset.GetValueOrDefault(customPageSize.GetValueOrDefault());

        return offset > 0
            ? instance.Skip(offset)
            : instance;
    }

    internal static PagedSelectCommandBuilder From(this PagedSelectCommandBuilder instance,
                                                   IQueryContext context,
                                                   IPagedDatabaseEntityRetrieverSettings settings)
        => instance.From(context.Query.GetTableName(settings.TableName));

    internal static PagedSelectCommandBuilder Where(this PagedSelectCommandBuilder instance,
                                                    IQueryContext context,
                                                    IPagedDatabaseEntityRetrieverSettings settings,
                                                    IQueryFieldInfo fieldInfo,
                                                    ISqlExpressionProvider sqlExpressionProvider,
                                                    ISqlConditionExpressionProvider provider,
                                                    ParameterBag parameterBag)
    {
        if (context.Query.Conditions.Count == 0 && string.IsNullOrEmpty(settings.DefaultWhere))
        {
            return instance;
        }

        if (!string.IsNullOrEmpty(settings.DefaultWhere))
        {
            instance.Where(settings.DefaultWhere);
        }

        foreach (var queryCondition in context.Query.Conditions)
        {
            provider.GetConditionExpression(
                context,
                queryCondition,
                fieldInfo,
                sqlExpressionProvider,
                parameterBag,
                (queryCondition.Combination ?? Combination.And) == Combination.And
                    ? instance.And
                    : instance.Or
            ).ThrowIfNotSuccessful();
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder OrderBy(this PagedSelectCommandBuilder instance,
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
            return instance;
        }
    }

    private static PagedSelectCommandBuilder AppendOrderBy(this PagedSelectCommandBuilder instance,
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

        return instance;
    }

    internal static PagedSelectCommandBuilder WithParameters(this PagedSelectCommandBuilder instance,
                                                             IParameterizedQuery? parameterizedQuery,
                                                             ParameterBag parameterBag)
    {
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

        return instance;
    }
}
