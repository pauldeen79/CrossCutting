namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringEndsWithConditionHandler : ConditionExpressionHandlerBase<StringEndsWithCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, StringEndsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, ("LIKE", "%{0}"));
}
