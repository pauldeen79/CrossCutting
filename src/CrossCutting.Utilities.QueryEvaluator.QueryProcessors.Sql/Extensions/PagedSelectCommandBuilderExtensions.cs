namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;

internal static class PagedSelectCommandBuilderExtensions
{
    internal static Task<Result<PagedSelectCommandBuilder>> Select(this PagedSelectCommandBuilder instance,
                                                                   PagedSelectCommandBuilderContext context)
    {
        var fieldSelectionQuery = context.QueryContext.Query as IFieldSelectionQuery;

        return fieldSelectionQuery is null || fieldSelectionQuery.GetAllFields
            ? instance.AppendSelectFieldsForAllFields(context.Settings, context.FieldInfo)
            : instance.AppendSelectFieldsForSpecifiedFields(context, fieldSelectionQuery);
    }

    private static Task<Result<PagedSelectCommandBuilder>> AppendSelectFieldsForAllFields(this PagedSelectCommandBuilder instance,
                                                                                          IPagedDatabaseEntityRetrieverSettings settings,
                                                                                          IQueryFieldInfo fieldInfo)
        => Task.Run(() =>
        {
            var allFields = fieldInfo.GetAllFields().ToArray();
            return Result.Success(allFields.Length != 0
                ? instance.Select(string.Join(", ", allFields
                    .Select(fieldInfo.GetDatabaseFieldName)
                    .Where(x => !string.IsNullOrEmpty(x))).WhenNullOrEmpty("*"))
                : instance.Select(settings.Fields.WhenNullOrWhitespace("*")));
        });

    private static async Task<Result<PagedSelectCommandBuilder>> AppendSelectFieldsForSpecifiedFields(this PagedSelectCommandBuilder instance,
                                                                                                      PagedSelectCommandBuilderContext context,
                                                                                                      IFieldSelectionQuery fieldSelectionQuery)
    {
        foreach (var expression in fieldSelectionQuery.FieldNames.Select((x, index) => new { Item = x, Index = index }))
        {
            if (expression.Index > 0)
            {
                instance.Select(", ");
            }

            var result = (await context.SqlExpressionProvider.GetSqlExpressionAsync(fieldSelectionQuery.WithContext(context.QueryContext), new SqlExpression(new PropertyNameEvaluatable(new ContextEvaluatable(), expression.Item)), context.FieldInfo, context.ParameterBag, CancellationToken.None).ConfigureAwait(false))
                .EnsureValue();
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

    internal static async Task<Result<PagedSelectCommandBuilder>> Where(this PagedSelectCommandBuilder instance,
                                                                        PagedSelectCommandBuilderContext context,
                                                                        ISqlConditionExpressionProvider provider,
                                                                        CancellationToken token)
    {
        if (context.QueryContext.Query.Conditions.Count == 0 && string.IsNullOrEmpty(context.Settings.DefaultWhere))
        {
            return Result.Success(instance);
        }

        if (!string.IsNullOrEmpty(context.Settings.DefaultWhere))
        {
            instance.Where(context.Settings.DefaultWhere);
        }

        foreach (var queryCondition in context.QueryContext.Query.Conditions)
        {
            var result = await provider.GetConditionExpressionAsync(
                context.QueryContext,
                queryCondition,
                context.FieldInfo,
                context.SqlExpressionProvider,
                context.ParameterBag,
                token
            ).ConfigureAwait(false);

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

    internal static async Task<Result<PagedSelectCommandBuilder>> OrderBy(this PagedSelectCommandBuilder instance,
                                                                          PagedSelectCommandBuilderContext context,
                                                                          CancellationToken token)
    {
        if (context.QueryContext.Query.SortOrders.Count > 0 || !string.IsNullOrEmpty(context.Settings.DefaultOrderBy))
        {
            return await instance.AppendOrderBy(context, token).ConfigureAwait(false);
        }
        else
        {
            return Result.Success(instance);
        }
    }

    private static async Task<Result<PagedSelectCommandBuilder>> AppendOrderBy(this PagedSelectCommandBuilder instance,
                                                                               PagedSelectCommandBuilderContext context,
                                                                               CancellationToken token)
    {
        foreach (var querySortOrder in context.QueryContext.Query.SortOrders.Select((x, index) => new { Item = x, Index = index }))
        {
            if (querySortOrder.Index > 0)
            {
                instance.OrderBy(", ");
            }

            var result = await context.SqlExpressionProvider.GetSqlExpressionAsync(context.QueryContext, new SqlExpression(querySortOrder.Item.Expression), context.FieldInfo, context.ParameterBag, token).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return Result.FromExistingResult<PagedSelectCommandBuilder>(result);
            }

            instance.OrderBy($"{result.Value} {querySortOrder.Item.ToSql()}");
        }

        if (context.QueryContext.Query.SortOrders.Count == 0 && !string.IsNullOrEmpty(context.Settings.DefaultOrderBy))
        {
            instance.OrderBy(context.Settings.DefaultOrderBy);
        }

        return Result.Success(instance);
    }

    internal static Result<PagedSelectCommandBuilder> AddParameters(this PagedSelectCommandBuilder instance,
                                                                    IQueryContext context,
                                                                    ParameterBag parameterBag)
    {
        foreach (var parameter in parameterBag.Parameters)
        {
            instance.AppendParameter(parameter.Key, parameter.Value!);
        }

        return Result.Success(instance);
    }
}
