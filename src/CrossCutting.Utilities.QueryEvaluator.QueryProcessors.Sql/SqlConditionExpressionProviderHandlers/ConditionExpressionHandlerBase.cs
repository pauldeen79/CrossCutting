namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviderHandlers;

public abstract class ConditionExpressionHandlerBase<TCondition> : ISqlConditionExpressionProviderHandler
{
    public Result GetConditionExpression(
        StringBuilder builder,
        IQueryContext context,
        ICondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag)
    {
        builder = ArgumentGuard.IsNotNull(builder, nameof(builder));
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (condition is not TCondition typedCondition)
        {
            return Result.Continue();
        }

        return DoGetConditionExpression(builder, context, typedCondition, fieldInfo, sqlExpressionProvider, parameterBag);
    }

    protected abstract Result DoGetConditionExpression(
        StringBuilder builder,
        IQueryContext context,
        TCondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag);

    protected static Result GetSimpleConditionExpression(StringBuilder builder, IQueryContext context, IDoubleExpressionContainer condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, ConditionParameters parameters)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .Add(nameof(condition.CompareExpression), () => sqlExpressionProvider.GetSqlExpression(context, new SqlExpression(condition.CompareExpression), fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} {parameters.Operator} {results.GetValue(nameof(condition.CompareExpression))}"));

    protected static Result GetStringConditionExpression(StringBuilder builder, IQueryContext context, IDoubleExpressionContainer condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, StringConditionParameters parameters)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .Add(nameof(condition.CompareExpression), () => sqlExpressionProvider.GetSqlExpression(context, new SqlLikeExpression(condition.CompareExpression, parameters.FormatString), fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} {parameters.Operator} {results.GetValue(nameof(condition.CompareExpression))}"));

    protected static Result GetInConditionExpression(StringBuilder builder, IQueryContext context, IInCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, string @operator)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .AddRange($"{nameof(condition.CompareExpressions)}.{{0}}", () => condition.CompareExpressions.Select(x => sqlExpressionProvider.GetSqlExpression(context, new SqlExpression(x), fieldInfo, parameterBag)))
            .Build()
            .OnSuccess(results =>
            {
                var secondExpressionValues = results
                    .Where(x => x.Key.StartsWith($"{nameof(condition.CompareExpressions)}."))
                    .Select(x => x.Value.Value);

                builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} {@operator} ({string.Join(", ", secondExpressionValues)})");
            });
}
