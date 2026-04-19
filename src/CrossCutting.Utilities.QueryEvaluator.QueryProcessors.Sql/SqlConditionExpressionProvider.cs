namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class SqlConditionExpressionProvider(IEnumerable<ISqlConditionExpressionProviderHandler> handlers) : ISqlConditionExpressionProvider
{
    private readonly ISqlConditionExpressionProviderHandler[] _handlers = ArgumentGuard.IsNotNull(handlers, nameof(handlers)).ToArray();

    public async Task<Result<string>> GetConditionExpressionAsync(
        object? context,
        ICondition condition,
        IEntityFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag,
        CancellationToken token)
    {
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        var builder = new StringBuilder();

        if (condition.StartGroup)
        {
            builder.Append("(");
        }

        Result result = Result.Invalid<string>($"No sql condition expression provider handler found for condition: {condition.GetType().FullName}");
        foreach (var handler in _handlers)
        {
            var handlerResult = await handler.GetConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, token).ConfigureAwait(false);
            if (handlerResult.Status != ResultStatus.Continue)
            {
                result = handlerResult;
                break;
            }
        }

        if (condition.EndGroup)
        {
            builder.Append(")");
        }

        return result.OnSuccess(_ => Result.Success(builder.ToString()));
    }
}
