namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotStartsWithConditionHandler : ConditionExpressionHandlerBase<StringNotStartsWithCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, StringNotStartsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("NOT LIKE", "{0}%"));
}
