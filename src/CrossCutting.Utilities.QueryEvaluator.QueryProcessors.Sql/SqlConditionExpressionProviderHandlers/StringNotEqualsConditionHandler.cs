namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotEqualsConditionHandler : ConditionExpressionHandlerBase<StringNotEqualsCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, StringNotEqualsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("<>", "{0}"));
}
