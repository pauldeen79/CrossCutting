namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class GreaterThanOrEqualConditionHandler : ConditionExpressionHandlerBase<GreaterThanOrEqualCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, object? context, GreaterThanOrEqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, query, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters(">="));
}
