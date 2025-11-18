namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class GreaterThanOrEqualConditionHandler : ConditionExpressionHandlerBase<GreaterThanOrEqualCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<GreaterThanOrEqualCondition> context, CancellationToken token)
        => GetSimpleConditionExpressionAsync(context, new ConditionParameters(">="), token);
}
