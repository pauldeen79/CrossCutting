namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class SqlConditionExpressionProvider : ISqlConditionExpressionProvider
{
    private readonly IEnumerable<ISqlConditionExpressionProviderHandler> _handlers;

    public SqlConditionExpressionProvider(IEnumerable<ISqlConditionExpressionProviderHandler> handlers)
    {
        ArgumentGuard.IsNotNull(handlers, nameof(handlers));
        _handlers = handlers;
    }

    public async Task<Result<string>> GetConditionExpressionAsync(
        IQueryContext context,
        ICondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
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
            var handlerResult = await handler.GetConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag).ConfigureAwait(false);
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
