namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringStartsWithConditionHandler : ConditionExpressionHandlerBase<StringStartsWithCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, object? context, StringStartsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, query, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("LIKE", "{0}%"));
}
