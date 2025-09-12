namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringEqualsConditionHandler : ConditionExpressionHandlerBase<StringEqualsCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, StringEqualsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("=", "{0}"));
}
