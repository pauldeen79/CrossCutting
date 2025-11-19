namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class SmallerThanConditionHandler : ConditionExpressionHandlerBase<SmallerThanCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<SmallerThanCondition> context, CancellationToken token)
        => GetSimpleConditionExpressionAsync(context, new ConditionParameters("<"), token);
}
