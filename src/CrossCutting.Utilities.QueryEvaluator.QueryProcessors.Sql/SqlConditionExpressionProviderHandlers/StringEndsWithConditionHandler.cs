namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringEndsWithConditionHandler : ConditionExpressionHandlerBase<StringEndsWithCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<StringEndsWithCondition> context, CancellationToken token)
        => GetStringConditionExpressionAsync(context, new StringConditionParameters("LIKE", "%{0}"), token);
}
