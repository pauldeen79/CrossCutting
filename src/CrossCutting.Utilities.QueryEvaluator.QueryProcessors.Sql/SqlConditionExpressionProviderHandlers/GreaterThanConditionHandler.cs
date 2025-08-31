namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class GreaterThanConditionHandler : ConditionExpressionHandlerBase<GreaterThanCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, GreaterThanCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, ">");
}
