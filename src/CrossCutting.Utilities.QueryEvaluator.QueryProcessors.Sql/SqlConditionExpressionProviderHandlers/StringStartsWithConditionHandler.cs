namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringStartsWithConditionHandler : ConditionExpressionHandlerBase<StringStartsWithCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, StringStartsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, query, condition, fieldInfo, sqlExpressionProvider, parameterBag, ("LIKE", "{0}%"));
}
