namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotEndsWithConditionHandler : ConditionExpressionHandlerBase<StringNotEndsWithCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, StringNotEndsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, CancellationToken token)
        => GetStringConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("NOT LIKE", "%{0}"), token);
}
