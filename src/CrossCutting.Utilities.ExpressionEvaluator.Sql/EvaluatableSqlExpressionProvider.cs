namespace CrossCutting.Utilities.ExpressionEvaluator.Sql;

public class EvaluatableSqlExpressionProvider : IEvaluatableSqlExpressionProvider, IEvaluatableSqlExpressionProviderHandler
{
    private readonly IEnumerable<IEvaluatableSqlExpressionProviderHandler> _handlers;

    public EvaluatableSqlExpressionProvider(IEnumerable<IEvaluatableSqlExpressionProviderHandler> handlers)
    {
        ArgumentGuard.IsNotNull(handlers, nameof(handlers));

        _handlers = handlers;
    }

    public async Task<Result> GetConditionExpressionAsync(SelectCommandBuilder selectCommandBuilder, object? context, IEvaluatable<bool> condition, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, CancellationToken token)
    {
        selectCommandBuilder = ArgumentGuard.IsNotNull(selectCommandBuilder, nameof(selectCommandBuilder));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldNameProvider = ArgumentGuard.IsNotNull(fieldNameProvider, nameof(fieldNameProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        foreach (var handler in _handlers)
        {
            var handlerResult = (await handler.GetConditionExpressionAsync(context, condition, fieldNameProvider, parameterBag, this, token).ConfigureAwait(false))
                .EnsureValue();
            if (!handlerResult.IsSuccessful())
            {
                return handlerResult;
            }

            if (handlerResult.Status == ResultStatus.Ok)
            {
                selectCommandBuilder
                    .Where(handlerResult.Value!)
                    .AppendParameters(parameterBag.Parameters);
                return handlerResult;
            }
        }

        return Result.NotSupported($"No evaluatable sql expression provider handler found for condition type: {condition.GetType().FullName}");
    }

    public async Task<Result<string>> GetConditionExpressionAsync(object? context, IEvaluatable evaluatable, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, IEvaluatableSqlExpressionProviderHandler callback, CancellationToken token)
    {
        evaluatable = ArgumentGuard.IsNotNull(evaluatable, nameof(evaluatable));

        foreach (var handler in _handlers)
        {
            var handlerResult = (await handler.GetConditionExpressionAsync(context, evaluatable, fieldNameProvider, parameterBag, this, token).ConfigureAwait(false))
                .EnsureValue();
            if (!handlerResult.IsSuccessful())
            {
                return handlerResult;
            }

            if (handlerResult.Status == ResultStatus.Ok)
            {
                //selectCommandBuilder.Where(handlerResult.Value!);
                return handlerResult;
            }
        }

        return Result.NotSupported<string>($"No evaluatable sql expression provider handler found for evaluatable type: {evaluatable.GetType().FullName}");
    }
}