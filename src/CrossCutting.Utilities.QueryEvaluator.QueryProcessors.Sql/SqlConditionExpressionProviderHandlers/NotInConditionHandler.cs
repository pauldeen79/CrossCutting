namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotInConditionHandler : ConditionExpressionHandlerBase<NotInCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, NotInCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetInConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, "NOT IN");
}
