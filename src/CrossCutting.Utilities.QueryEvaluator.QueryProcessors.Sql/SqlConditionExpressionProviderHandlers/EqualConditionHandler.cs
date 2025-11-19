namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class EqualConditionHandler : ConditionExpressionHandlerBase<EqualCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<EqualCondition> context, CancellationToken token)
        => GetSimpleConditionExpressionAsync(context, new ConditionParameters("="), token);
}
