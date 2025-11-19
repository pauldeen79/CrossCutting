namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class InConditionHandler : ConditionExpressionHandlerBase<InCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<InCondition> context, CancellationToken token)
        => GetInConditionExpressionAsync(context, "IN", token);
}
