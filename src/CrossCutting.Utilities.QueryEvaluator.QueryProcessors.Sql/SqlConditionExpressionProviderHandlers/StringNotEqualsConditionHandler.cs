namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotEqualsConditionHandler : ConditionExpressionHandlerBase<StringNotEqualsCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<StringNotEqualsCondition> context, CancellationToken token)
        => GetStringConditionExpressionAsync(context, new StringConditionParameters("<>", "{0}"), token);
}
