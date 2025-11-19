namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringEqualsConditionHandler : ConditionExpressionHandlerBase<StringEqualsCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<StringEqualsCondition> context, CancellationToken token)
        => GetStringConditionExpressionAsync(context, new StringConditionParameters("=", "{0}"), token);
}
