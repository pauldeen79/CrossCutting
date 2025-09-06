namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviderHandlers;

public abstract class ConditionExpressionHandlerBase<TCondition> : ISqlConditionExpressionProviderHandler
{
    public Result GetConditionExpression(
        StringBuilder builder,
        IQuery query,
        object? context,
        ICondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag)
    {
        builder = ArgumentGuard.IsNotNull(builder, nameof(builder));
        query = ArgumentGuard.IsNotNull(query, nameof(query));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (condition is not TCondition typedCondition)
        {
            return Result.Continue();
        }

        return DoGetConditionExpression(builder, query, context, typedCondition, fieldInfo, sqlExpressionProvider, parameterBag);
    }

    protected abstract Result DoGetConditionExpression(
        StringBuilder builder,
        IQuery query,
        object? context,
        TCondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag);

    protected static Result GetSimpleConditionExpression(StringBuilder builder, IQuery query, object? context, IDoubleExpressionContainer condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, ConditionParameters parameters)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(query, context, condition.SourceExpression, fieldInfo, parameterBag))
            .Add(nameof(condition.CompareExpression), () => sqlExpressionProvider.GetSqlExpression(query, context, condition.CompareExpression, fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} {parameters.Operator} {results.GetValue(nameof(condition.CompareExpression))}"));

    protected static Result GetStringConditionExpression(StringBuilder builder, IQuery query, object? context, IDoubleExpressionContainer condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, StringConditionParameters parameters)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(query, context, condition.SourceExpression, fieldInfo, parameterBag))
            .Add(nameof(condition.CompareExpression), () => sqlExpressionProvider.GetSqlExpression(query, context, new SqlLikeExpression(condition.CompareExpression, parameters.FormatString), fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} {parameters.Operator} {results.GetValue(nameof(condition.CompareExpression))}"));

    protected static Result GetInConditionExpression(StringBuilder builder, IQuery query, object? context, IInCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, string @operator)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(query, context, condition.SourceExpression, fieldInfo, parameterBag))
            .AddRange($"{nameof(condition.CompareExpressions)}.{{0}}", () => condition.CompareExpressions.Select(x => sqlExpressionProvider.GetSqlExpression(query, context, x, fieldInfo, parameterBag)))
            .Build()
            .OnSuccess(results =>
            {
                var secondExpressionValues = results
                    .Where(x => x.Key.StartsWith($"{nameof(condition.CompareExpressions)}."))
                    .Select(x => x.Value.Value);

                builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} {@operator} ({string.Join(", ", secondExpressionValues)})");
            });
}
