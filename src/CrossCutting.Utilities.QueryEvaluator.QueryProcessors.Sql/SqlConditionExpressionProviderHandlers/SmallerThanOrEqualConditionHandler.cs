namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class SmallerThanOrEqualConditionHandler : ConditionExpressionHandlerBase<SmallerThanOrEqualCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, SmallerThanOrEqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("<="));
}
