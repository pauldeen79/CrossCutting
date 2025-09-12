namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotEndsWithConditionHandler : ConditionExpressionHandlerBase<StringNotEndsWithCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, StringNotEndsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("NOT LIKE", "%{0}"));
}
