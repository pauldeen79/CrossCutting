namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotStartsWithConditionHandler : ConditionExpressionHandlerBase<StringNotStartsWithCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, object? context, StringNotStartsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, query, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("NOT LIKE", "{0}%"));
}
