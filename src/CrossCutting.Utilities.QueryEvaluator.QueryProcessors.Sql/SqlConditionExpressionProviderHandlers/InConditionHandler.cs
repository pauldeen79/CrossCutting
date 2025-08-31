namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class InConditionHandler : ConditionExpressionHandlerBase<InCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, InCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetInConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, "IN");
}
