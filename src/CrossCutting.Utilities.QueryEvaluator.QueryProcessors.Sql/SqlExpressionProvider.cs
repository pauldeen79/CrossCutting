namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class SqlExpressionProvider : ISqlExpressionProvider
{
    private readonly IEnumerable<ISqlExpressionProviderHandler> _handlers;

    public SqlExpressionProvider(IEnumerable<ISqlExpressionProviderHandler> handlers)
    {
        ArgumentGuard.IsNotNull(handlers, nameof(handlers));

        _handlers = handlers;
    }

    public async Task<Result<string>> GetSqlExpressionAsync(IQueryContext context, ISqlExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        foreach (var handler in _handlers)
        {
            var result = await handler.GetSqlExpressionAsync(context, expression, fieldInfo, parameterBag, this, token).ConfigureAwait(false);
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }
        
        return Result.Invalid<string>($"No sql expression provider handler found for type: {expression.GetType().FullName}");
    }
}
