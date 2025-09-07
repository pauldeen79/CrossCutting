namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class GreaterThanConditionHandler : ConditionExpressionHandlerBase<GreaterThanCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, GreaterThanCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters(">"));
}
