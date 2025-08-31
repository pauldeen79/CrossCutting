namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotContainsConditionHandler : ConditionExpressionHandlerBase<StringNotContainsCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, StringNotContainsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, ("NOT LIKE", "%{0}%"));
}
