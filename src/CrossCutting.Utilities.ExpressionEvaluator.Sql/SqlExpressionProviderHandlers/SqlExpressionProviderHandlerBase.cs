namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.SqlExpressionProviderHandlers;

public abstract class SqlExpressionProviderHandlerBase<TExpression> : ISqlExpressionProviderHandler
    where TExpression : ISqlExpression
{
    public async Task<Result<string>> GetSqlExpressionAsync(
        object? context,
        ISqlExpression expression,
        IFieldNameProvider fieldNameProvider,
        ParameterBag parameterBag,
        ISqlExpressionProvider callback,
        CancellationToken token)
    {
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldNameProvider = ArgumentGuard.IsNotNull(fieldNameProvider, nameof(fieldNameProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        callback = ArgumentGuard.IsNotNull(callback, nameof(callback));

        if (expression is not TExpression typedExpression)
        {
            return Result.Continue<string>();
        }

        return await HandleGetSqlExpressionAsync(context, typedExpression, fieldNameProvider, parameterBag, callback, token).ConfigureAwait(false);
    }

    protected abstract Task<Result<string>> HandleGetSqlExpressionAsync(
        object? context,
        TExpression expression,
        IFieldNameProvider fieldNameProvider,
        ParameterBag parameterBag,
        ISqlExpressionProvider callback,
        CancellationToken token);
}
