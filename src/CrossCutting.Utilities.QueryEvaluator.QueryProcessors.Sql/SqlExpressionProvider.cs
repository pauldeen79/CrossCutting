namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class SqlExpressionProvider : ISqlExpressionProvider
{
    private readonly IEnumerable<ISqlExpressionProviderHandler> _handlers;

    public SqlExpressionProvider(IEnumerable<ISqlExpressionProviderHandler> handlers)
    {
        ArgumentGuard.IsNotNull(handlers, nameof(handlers));
        _handlers = handlers;
    }

    public Result<string> GetSqlExpression(IQueryContext context, IEvaluatable expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        return _handlers
            .Select(x => x.GetSqlExpression(context, expression, fieldInfo, parameterBag, this))
            .WhenNotContinue(() => Result.Invalid<string>($"No sql expression provider handler found for type: {expression.GetType().FullName}"));
    }
}
