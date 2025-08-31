namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringContainsConditionHandler : ConditionExpressionHandlerBase<StringContainsCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, StringContainsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, ("LIKE", "%{0}%"));
}
