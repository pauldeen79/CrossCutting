namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringEqualsConditionHandler : ConditionExpressionHandlerBase<StringEqualsCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, object? context, StringEqualsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetStringConditionExpression(builder, query, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("=", "{0}"));
}
