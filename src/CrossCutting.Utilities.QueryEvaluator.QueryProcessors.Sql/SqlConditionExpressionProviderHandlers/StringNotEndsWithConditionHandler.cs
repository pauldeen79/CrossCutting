namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotEndsWithConditionHandler : ConditionExpressionHandlerBase<StringNotEndsWithCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, StringNotEndsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("NOT LIKE", "%{0}"));
}
