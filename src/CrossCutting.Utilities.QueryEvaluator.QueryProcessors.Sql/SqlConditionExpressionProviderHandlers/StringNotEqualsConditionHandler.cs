namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotEqualsConditionHandler : ConditionExpressionHandlerBase<StringNotEqualsCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, StringNotEqualsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, CancellationToken token)
        => GetStringConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("<>", "{0}"), token);
}
