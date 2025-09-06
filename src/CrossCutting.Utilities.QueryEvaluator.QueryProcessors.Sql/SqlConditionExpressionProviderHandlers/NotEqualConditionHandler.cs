namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotEqualConditionHandler : ConditionExpressionHandlerBase<NotEqualCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, object? context, NotEqualCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, query, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters("<>"));
}
