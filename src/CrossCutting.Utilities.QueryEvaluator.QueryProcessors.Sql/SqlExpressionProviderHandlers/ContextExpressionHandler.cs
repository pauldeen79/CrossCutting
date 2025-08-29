namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class ContextExpressionHandler : SqlExpressionProviderHandlerBase<ContextExpression>
{
    protected override Result<string> DoGetSqlExpression(IQuery query, ContextExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag)
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        return Result.Success(parameterBag.CreateQueryParameterName(query.GetContext()));
    }
}
