namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringEndsWithConditionHandler : ConditionExpressionHandlerBase<StringEndsWithCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, StringEndsWithCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, CancellationToken token)
        => GetStringConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("LIKE", "%{0}"), token);
}
