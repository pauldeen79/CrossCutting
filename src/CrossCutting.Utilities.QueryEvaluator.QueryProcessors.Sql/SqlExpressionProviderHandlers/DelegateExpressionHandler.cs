namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class DelegateExpressionHandler : SqlExpressionProviderHandlerBase<DelegateExpression>
{
    protected override Result<string> DoGetSqlExpression(IQuery query, object? context, DelegateExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));

        return Result.WrapException(() =>  Result.Success(parameterBag.CreateQueryParameterName(expression.Value())));
    }
}
