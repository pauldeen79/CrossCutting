namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringEndsWithConditionHandler : ConditionExpressionHandlerBase<StringEndsWithCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, StringEndsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("LIKE", "%{0}"));
}
