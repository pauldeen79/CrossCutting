namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public abstract class SqlExpressionProviderHandlerBase<TExpression> : ISqlExpressionProviderHandler
    where TExpression : ISqlExpression
{
    public async Task<Result<string>> GetSqlExpressionAsync(
        IQueryContext context,
        ISqlExpression expression,
        IQueryFieldInfo fieldInfo,
        ParameterBag parameterBag,
        ISqlExpressionProvider callback,
        CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        callback = ArgumentGuard.IsNotNull(callback, nameof(callback));

        if (expression is not TExpression typedExpression)
        {
            return Result.Continue<string>();
        }

        return await DoGetSqlExpressionAsync(context, typedExpression, fieldInfo, parameterBag, callback, token).ConfigureAwait(false);
    }

    protected abstract Task<Result<string>> DoGetSqlExpressionAsync(
        IQueryContext context,
        TExpression expression,
        IQueryFieldInfo fieldInfo,
        ParameterBag parameterBag,
        ISqlExpressionProvider callback,
        CancellationToken token);
}
