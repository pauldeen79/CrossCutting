namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class ContextExpressionHandler : SqlExpressionProviderHandlerBase<ContextExpression>
{
    protected override Result<string> DoGetSqlExpression(IQueryContext context, ContextExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        return Result.Success(parameterBag.CreateQueryParameterName(context));
    }
}
