namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NotEqualConditionHandler : ConditionExpressionHandlerBase<NotEqualCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<NotEqualCondition> context, CancellationToken token)
        => GetSimpleConditionExpressionAsync(context, new ConditionParameters("<>"), token);
}
