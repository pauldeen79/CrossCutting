namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringEqualsConditionHandler : ConditionExpressionHandlerBase<StringEqualsCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, StringEqualsCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag, CancellationToken token)
        => GetStringConditionExpressionAsync(builder, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new StringConditionParameters("=", "{0}"), token);
}
