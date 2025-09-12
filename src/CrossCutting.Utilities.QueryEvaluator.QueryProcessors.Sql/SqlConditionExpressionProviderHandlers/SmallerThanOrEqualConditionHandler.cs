namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class SmallerThanOrEqualConditionHandler : ConditionExpressionHandlerBase<SmallerThanOrEqualCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, SmallerThanOrEqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("<="));
}
