namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class GreaterThanConditionHandler : ConditionExpressionHandlerBase<GreaterThanCondition>
{
    protected override Task<Result> GetConditionExpressionAsync(ConditionExpressionHandlerContext<GreaterThanCondition> context, CancellationToken token)
        => GetSimpleConditionExpressionAsync(context, new ConditionParameters(">"), token);
}
