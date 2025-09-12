namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotContainsConditionHandler : ConditionExpressionHandlerBase<StringNotContainsCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, StringNotContainsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("NOT LIKE", "%{0}%"));
}
