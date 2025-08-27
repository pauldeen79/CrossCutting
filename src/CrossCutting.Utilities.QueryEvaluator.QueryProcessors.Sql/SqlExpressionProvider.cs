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

        return _handlers
            .Select(x => x.GetSqlExpression(query, expression, fieldInfo, parameterBag))
            .WhenNotContinue(() => Result.Invalid<string>($"No sql expression provider handler found for type: {query.GetType().FullName}"));
    }
}
