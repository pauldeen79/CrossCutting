namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class InConditionHandler : ConditionExpressionHandlerBase<InCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, InCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetInConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, "IN");
}
