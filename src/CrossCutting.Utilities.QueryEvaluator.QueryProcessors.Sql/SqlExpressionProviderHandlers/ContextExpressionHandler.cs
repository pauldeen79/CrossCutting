namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class ContextExpressionHandler : SqlExpressionProviderHandlerBase<ContextEvaluatable>
{
    protected override Result<string> DoGetSqlExpression(IQueryContext context, ContextEvaluatable expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        return Result.Success(parameterBag.CreateQueryParameterName(context.Context));
    }
}
