namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class SqlExpressionProvider : ISqlExpressionProvider
{
    private readonly IEnumerable<ISqlExpressionProviderHandler> _handlers;

    public SqlExpressionProvider(IEnumerable<ISqlExpressionProviderHandler> handlers)
    {
        ArgumentGuard.IsNotNull(handlers, nameof(handlers));
        _handlers = handlers;
    }

    public Result<string> GetSqlExpression(IQuery query, IExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag)
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        return _handlers
            .Select(x => x.GetSqlExpression(query, expression, fieldInfo, parameterBag))
            .WhenNotContinue(() => Result.Invalid<string>($"No sql expression provider handler found for type: {expression.GetType().FullName}"));
    }
}
