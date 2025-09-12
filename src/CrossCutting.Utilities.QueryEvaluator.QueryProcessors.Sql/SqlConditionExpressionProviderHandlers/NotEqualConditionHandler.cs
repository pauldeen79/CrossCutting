namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotEqualConditionHandler : ConditionExpressionHandlerBase<NotEqualCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, NotEqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("<>"));
}
