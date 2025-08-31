namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class SmallerThanConditionHandler : ConditionExpressionHandlerBase<SmallerThanCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, SmallerThanCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, "<");
}
