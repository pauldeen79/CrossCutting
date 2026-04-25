namespace CrossCutting.Utilities.ExpressionEvaluator.Sql;

public class SqlExpressionProvider(IEnumerable<ISqlExpressionProviderHandler> handlers) : ISqlExpressionProvider
{
    private readonly ISqlExpressionProviderHandler[] _handlers = ArgumentGuard.IsNotNull(handlers, nameof(handlers)).ToArray();

    public async Task<Result<string>> GetSqlExpressionAsync(object? context, ISqlExpression expression, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, CancellationToken token)
    {
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldNameProvider = ArgumentGuard.IsNotNull(fieldNameProvider, nameof(fieldNameProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        foreach (var handler in _handlers)
        {
            var result = await handler.GetSqlExpressionAsync(context, expression, fieldNameProvider, parameterBag, this, token).ConfigureAwait(false);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }
        
        return Result.Invalid<string>($"No sql expression provider handler found for type: {expression.GetType().FullName}");
    }
}
