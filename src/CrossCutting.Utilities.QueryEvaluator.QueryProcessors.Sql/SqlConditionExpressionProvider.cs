namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class SqlConditionExpressionProvider : ISqlConditionExpressionProvider
{
    private readonly IEnumerable<ISqlConditionExpressionProviderHandler> _handlers;

    public SqlConditionExpressionProvider(IEnumerable<ISqlConditionExpressionProviderHandler> handlers)
    {
        ArgumentGuard.IsNotNull(handlers, nameof(handlers));
        _handlers = handlers;
    }

    public Result GetConditionExpression(
        IQuery query,
        object? context,
        ICondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag,
        Func<string, PagedSelectCommandBuilder> actionDelegate)
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        actionDelegate = ArgumentGuard.IsNotNull(actionDelegate, nameof(actionDelegate));

        var builder = new StringBuilder();

        if (condition.StartGroup)
        {
            builder.Append("(");
        }

        var result = _handlers
            .Select(x => x.GetConditionExpression(builder, query, context, condition, fieldInfo, sqlExpressionProvider, parameterBag))
            .WhenNotContinue(() => Result.Invalid<string>($"No sql condition expression provider handler found for condition: {condition.GetType().FullName}"));

        if (condition.EndGroup)
        {
            builder.Append(")");
        }

        actionDelegate.Invoke(builder.ToString());

        return result;
    }
}
