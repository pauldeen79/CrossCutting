namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;

internal static class PagedSelectCommandBuilderExtensions
{
    internal static PagedSelectCommandBuilder Select(this PagedSelectCommandBuilder instance,
                                                     IPagedDatabaseEntityRetrieverSettings settings,
                                                     IQueryFieldInfo fieldInfo,
                                                     IFieldSelectionQuery? fieldSelectionQuery,
                                                     ISqlExpressionProvider sqlExpressionProvider,
                                                     ParameterBag parameterBag)
        => fieldSelectionQuery is null || fieldSelectionQuery.GetAllFields
            ? instance.AppendSelectFieldsForAllFields(settings, fieldInfo)
            : instance.AppendSelectFieldsForSpecifiedFields(fieldSelectionQuery, fieldInfo, sqlExpressionProvider, parameterBag);

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

            instance.Select(sqlExpressionProvider.GetSqlExpression(fieldSelectionQuery, new PropertyNameExpressionBuilder(expression.Item).Build(), fieldInfo, parameterBag).GetValueOrThrow());
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder Distinct(this PagedSelectCommandBuilder instance,
                                                       IFieldSelectionQuery? fieldSelectionQuery)
        => instance.DistinctValues(fieldSelectionQuery?.Distinct == true);

    internal static PagedSelectCommandBuilder Top(this PagedSelectCommandBuilder instance,
                                                  IQuery query,
                                                  IPagedDatabaseEntityRetrieverSettings settings,
                                                  int? customLimit)
    {
        var limit = query.Limit.IfNotGreaterThan(settings.OverridePageSize, customLimit);

        return limit > 0
            ? instance.WithTop(limit)
            : instance;
    }

    internal static PagedSelectCommandBuilder Offset(this PagedSelectCommandBuilder instance,
                                                     IQuery query,
                                                     int? customPageSize)
    {
        var offset = query.Offset.GetValueOrDefault(customPageSize.GetValueOrDefault());

        return offset > 0
            ? instance.Skip(offset)
            : instance;
    }

    internal static PagedSelectCommandBuilder From(this PagedSelectCommandBuilder instance,
                                                   IQuery query,
                                                   IPagedDatabaseEntityRetrieverSettings settings)
        => instance.From(query.GetTableName(settings.TableName));

    internal static PagedSelectCommandBuilder Where(this PagedSelectCommandBuilder instance,
                                                    IQuery query,
                                                    IPagedDatabaseEntityRetrieverSettings settings,
                                                    IQueryFieldInfo fieldInfo,
                                                    ISqlExpressionProvider sqlExpressionProvider,
                                                    ParameterBag parameterBag)
    {
        if (query.Conditions.Count == 0 && string.IsNullOrEmpty(settings.DefaultWhere))
        {
            return instance;
        }

        if (!string.IsNullOrEmpty(settings.DefaultWhere))
        {
            instance.Where(settings.DefaultWhere);
        }

        foreach (var queryCondition in query.Conditions)
        {
            instance.AppendQueryCondition
            (
                query,
                queryCondition,
                fieldInfo,
                sqlExpressionProvider,
                parameterBag,
                (queryCondition.Combination ?? Combination.And) == Combination.And
                    ? instance.And
                    : instance.Or
            );
        }

        return instance;
    }

    internal static PagedSelectCommandBuilder OrderBy(this PagedSelectCommandBuilder instance,
                                                      IQuery query,
                                                      IPagedDatabaseEntityRetrieverSettings settings,
                                                      IQueryFieldInfo fieldInfo,
                                                      ISqlExpressionProvider sqlExpressionProvider,
                                                      ParameterBag parameterBag)
    {
        if (query.Offset.HasValue && query.Offset.Value >= 0)
        {
            //do not use order by (this will be taken care of by the row_number Expression)
            return instance;
        }
        else if (query.SortOrders.Count > 0 || !string.IsNullOrEmpty(settings.DefaultOrderBy))
        {
            return instance.AppendOrderBy(query, settings, fieldInfo, sqlExpressionProvider, parameterBag);
        }
        else
        {
            return instance;
        }
    }

    private static PagedSelectCommandBuilder AppendOrderBy(this PagedSelectCommandBuilder instance,
                                                           IQuery query,
                                                           IPagedDatabaseEntityRetrieverSettings settings,
                                                           IQueryFieldInfo fieldInfo,
                                                           ISqlExpressionProvider sqlExpressionProvider,
                                                           ParameterBag parameterBag)
    {
        foreach (var querySortOrder in query.SortOrders.Select((x, index) => new { Item = x, Index = index }))
        {
            if (querySortOrder.Index > 0)
            {
                instance.OrderBy(", ");
            }

            instance.OrderBy($"{sqlExpressionProvider.GetSqlExpression(query, querySortOrder.Item.Expression, fieldInfo, parameterBag).GetValueOrThrow()} {querySortOrder.Item.ToSql()}");
        }

        if (query.SortOrders.Count == 0 && !string.IsNullOrEmpty(settings.DefaultOrderBy))
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

    internal static PagedSelectCommandBuilder AppendQueryCondition(this PagedSelectCommandBuilder instance,
                                                                   IQuery query,
                                                                   ICondition condition,
                                                                   IQueryFieldInfo fieldInfo,
                                                                   ISqlExpressionProvider sqlExpressionProvider,
                                                                   ParameterBag parameterBag,
                                                                   Func<string, PagedSelectCommandBuilder> actionDelegate)
    {
        var builder = new StringBuilder();

        if (condition.StartGroup)
        {
            builder.Append("(");
        }

        //if (!condition.Operator.GetType().In(typeof(StringContainsOperator),
        //                                     typeof(StringNotContainsOperator),
        //                                     typeof(EndsWithOperator),
        //                                     typeof(NotEndsWithOperator),
        //                                     typeof(StartsWithOperator),
        //                                     typeof(NotStartsWithOperator)))
        //{
        //    builder.Append(condition.Operator.ToNot());

        //    if (condition.Operator.GetType().In(typeof(IsNullOrEmptyOperator), typeof(IsNotNullOrEmptyOperator)))
        //    {
        //        builder.Append("COALESCE(");
        //    }
        //    else if (condition.Operator.GetType().In(typeof(IsNullOrWhiteSpaceOperator), typeof(IsNotNullOrWhiteSpaceOperator)))
        //    {
        //        builder.Append("COALESCE(TRIM(");
        //    }

        //    builder.Append(evaluator.GetSqlExpression(query, condition.LeftExpression, fieldInfo, parameterBag));

        //    if (condition.Operator.GetType().In(typeof(IsNullOrEmptyOperator), typeof(IsNotNullOrEmptyOperator)))
        //    {
        //        builder.Append(",'')");
        //    }
        //    else if (condition.Operator.GetType().In(typeof(IsNullOrWhiteSpaceOperator), typeof(IsNotNullOrWhiteSpaceOperator)))
        //    {
        //        builder.Append("),'')");
        //    }
        //}

        //AppendOperatorAndValue(condition, query, fieldInfo, builder, sqlExpressionProvider, parameterBag);

        if (condition.EndGroup)
        {
            builder.Append(")");
        }

        actionDelegate.Invoke(builder.ToString());

        return instance;
    }

    //private static void AppendOperatorAndValue(ICondition condition,
    //                                           IQuery query,
    //                                           IQueryFieldInfo fieldInfo,
    //                                           StringBuilder builder,
    //                                           ISqlExpressionProvider sqlExpressionProvider,
    //                                           ParameterBag parameterBag)
    //{
    //    var leftExpressionSql = new Func<string>(() => sqlExpressionProvider.GetSqlExpression(query, condition.LeftExpression, fieldInfo, parameterBag));
    //    var rightExpressionSql = new Func<string>(() => sqlExpressionProvider.GetSqlExpression(query, condition.RightExpression, fieldInfo, parameterBag));
    //    var length = new Func<string>(() => sqlExpressionProvider.GetLengthExpression(query, condition.RightExpression, fieldInfo));

    //    var sqlToAppend = condition.Operator switch
    //    {
    //        IsNullOperator => " IS NULL",
    //        IsNotNullOperator => " IS NOT NULL",
    //        IsNullOrEmptyOperator => " = ''",
    //        IsNotNullOrEmptyOperator => " <> ''",
    //        IsNullOrWhiteSpaceOperator => " = ''",
    //        IsNotNullOrWhiteSpaceOperator => " <> ''",
    //        StringContainsOperator => $"CHARINDEX({rightExpressionSql()}, {leftExpressionSql()}) > 0",
    //        StringNotContainsOperator => $"CHARINDEX({rightExpressionSql()}, {leftExpressionSql()}) = 0",
    //        StartsWithOperator => $"LEFT({leftExpressionSql()}, {length()}) = {rightExpressionSql()}",
    //        NotStartsWithOperator => $"LEFT({leftExpressionSql()}, {length()}) <> {rightExpressionSql()}",
    //        EndsWithOperator => $"RIGHT({leftExpressionSql()}, {length()}) = {rightExpressionSql()}",
    //        NotEndsWithOperator => $"RIGHT({leftExpressionSql()}, {length()}) <> {rightExpressionSql()}",
    //        _ => $" {condition.Operator.ToSql()} {rightExpressionSql()}"
    //    };

    //    builder.Append(sqlToAppend);
    //}
}
