namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringContainsConditionHandler : ConditionExpressionHandlerBase<StringContainsCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<StringContainsCondition> context, CancellationToken token)
        => GetStringConditionExpressionAsync(context, new StringConditionParameters("LIKE", "%{0}%"), token);
}
