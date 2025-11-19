namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class GreaterThanConditionHandler : ConditionExpressionHandlerBase<GreaterThanCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<GreaterThanCondition> context, CancellationToken token)
        => GetSimpleConditionExpressionAsync(context, new ConditionParameters(">"), token);
}
