namespace CrossCutting.Utilities.ExpressionEvaluator.Sql;

public class EvaluatableSqlExpressionProvider(IEnumerable<IEvaluatableSqlExpressionProviderHandler> handlers) : IEvaluatableSqlExpressionProvider, IEvaluatableSqlExpressionProviderHandler
{
    private readonly IEvaluatableSqlExpressionProviderHandler[] _handlers = ArgumentGuard.IsNotNull(handlers, nameof(handlers)).ToArray();

    public async Task<Result<SqlExpression>> GetExpressionAsync(IEvaluatable<bool> condition, IFieldNameProvider fieldNameProvider, object? context, CancellationToken token)
    {
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldNameProvider = ArgumentGuard.IsNotNull(fieldNameProvider, nameof(fieldNameProvider));
        
        var parameterBag = new ParameterBag();

        foreach (var handler in _handlers)
        {
            var handlerResult = (await handler.GetExpressionAsync(context, condition, fieldNameProvider, parameterBag, this, token).ConfigureAwait(false))
                .EnsureValue();
            if (!handlerResult.IsSuccessful())
            {
                return Result.FromExistingResult<SqlExpression>(handlerResult);
            }

            if (handlerResult.Status == ResultStatus.Ok)
            {
                return new SqlExpression(handlerResult.Value!, parameterBag.Parameters);
            }
        }

        return Result.NotSupported<SqlExpression>($"No evaluatable sql expression provider handler found for condition type: {condition.GetType().FullName}");
    }

    public async Task<Result<string>> GetExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        evaluatable = ArgumentGuard.IsNotNull(evaluatable, nameof(evaluatable));

        foreach (var handler in _handlers)
        {
            var handlerResult = (await handler.GetExpressionAsync(context, evaluatable, fieldNameProvider, parameterBag, this, token).ConfigureAwait(false))
                .EnsureValue();
            if (handlerResult.Status != ResultStatus.Continue)
            {
                return handlerResult;
            }
        }

        return Result.NotSupported<string>($"No evaluatable sql expression provider handler found for evaluatable type: {evaluatable.GetType().FullName}");
    }
}