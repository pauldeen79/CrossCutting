namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotEqualConditionHandler : ConditionExpressionHandlerBase<NotEqualCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, NotEqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("<>"));
}
