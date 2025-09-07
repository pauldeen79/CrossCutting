namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class SmallerThanConditionHandler : ConditionExpressionHandlerBase<SmallerThanCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, SmallerThanCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("<"));
}
