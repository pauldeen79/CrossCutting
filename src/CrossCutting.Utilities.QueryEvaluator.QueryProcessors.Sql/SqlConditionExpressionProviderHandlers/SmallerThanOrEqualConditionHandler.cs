namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class SmallerThanOrEqualConditionHandler : ConditionExpressionHandlerBase<SmallerThanOrEqualCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<SmallerThanOrEqualCondition> context, CancellationToken token)
        => GetSimpleConditionExpressionAsync(context, new ConditionParameters("<="), token);
}
