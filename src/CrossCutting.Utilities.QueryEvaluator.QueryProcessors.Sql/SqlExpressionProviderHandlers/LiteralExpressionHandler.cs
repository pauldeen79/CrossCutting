namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class LiteralExpressionHandler : SqlExpressionProviderHandlerBase<LiteralExpression>
{
    protected override Result<string> DoGetSqlExpression(IQuery query, LiteralExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));

        return Result.Success(parameterBag.CreateQueryParameterName(expression.Value));
    }
}
