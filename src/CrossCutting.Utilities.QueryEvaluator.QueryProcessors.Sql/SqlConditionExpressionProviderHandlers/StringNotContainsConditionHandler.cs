namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotContainsConditionHandler : ConditionExpressionHandlerBase<StringNotContainsCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<StringNotContainsCondition> context, CancellationToken token)
        => GetStringConditionExpressionAsync(context, new StringConditionParameters("NOT LIKE", "%{0}%"), token);
}
