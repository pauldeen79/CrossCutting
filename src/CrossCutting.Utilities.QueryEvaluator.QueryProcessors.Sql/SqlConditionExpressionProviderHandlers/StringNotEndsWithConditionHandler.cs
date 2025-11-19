namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotEndsWithConditionHandler : ConditionExpressionHandlerBase<StringNotEndsWithCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<StringNotEndsWithCondition> context, CancellationToken token)
        => GetStringConditionExpressionAsync(context, new StringConditionParameters("NOT LIKE", "%{0}"), token);
}
