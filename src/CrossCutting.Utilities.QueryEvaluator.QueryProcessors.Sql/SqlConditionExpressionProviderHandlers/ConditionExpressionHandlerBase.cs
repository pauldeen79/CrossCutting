namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviderHandlers;

public abstract class ConditionExpressionHandlerBase<TCondition> : ISqlConditionExpressionProviderHandler
{
    public async Task<Result> GetConditionExpressionAsync(
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

        return await DoGetConditionExpressionAsync(builder, context, typedCondition, fieldInfo, sqlExpressionProvider, parameterBag).ConfigureAwait(false);
    }

    protected abstract Task<Result> DoGetConditionExpressionAsync(
        StringBuilder builder,
        IQueryContext context,
        TCondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag);

    protected static async Task<Result> GetSimpleConditionExpressionAsync(StringBuilder builder, IQueryContext context, IDoubleExpressionContainer condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, ConditionParameters parameters)
    {
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .Add(nameof(condition.CompareExpression), sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(condition.CompareExpression), fieldInfo, parameterBag))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} {parameters.Operator} {results.GetValue(nameof(condition.CompareExpression))}"));
    }

    protected static async Task<Result> GetStringConditionExpressionAsync(StringBuilder builder, IQueryContext context, IDoubleExpressionContainer condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, StringConditionParameters parameters)
    {
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        parameters = ArgumentGuard.IsNotNull(parameters, nameof(parameters));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .Add(nameof(condition.CompareExpression), sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlLikeExpression(condition.CompareExpression, parameters.FormatString), fieldInfo, parameterBag))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} {parameters.Operator} {results.GetValue(nameof(condition.CompareExpression))}"));
    }

    protected static async Task<Result> GetInConditionExpressionAsync(StringBuilder builder, IQueryContext context, IInCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, string @operator)
    {
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .AddRange($"{nameof(condition.CompareExpressions)}.{{0}}", condition.CompareExpressions.Select(x => sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(x), fieldInfo, parameterBag)))
            .Build().ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var secondExpressionValues = results
                    .Where(x => x.Key.StartsWith($"{nameof(condition.CompareExpressions)}."))
                    .Select(x => x.Value.Value);

                builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} {@operator} ({string.Join(", ", secondExpressionValues)})");
            });
    }
}
