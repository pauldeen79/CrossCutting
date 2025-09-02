namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class EqualConditionHandler : ConditionExpressionHandlerBase<EqualCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, EqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("="));
}
