namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringStartsWithConditionHandler : ConditionExpressionHandlerBase<StringStartsWithCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<StringStartsWithCondition> context, CancellationToken token)
        => GetStringConditionExpressionAsync(context, new StringConditionParameters("LIKE", "{0}%"), token);
}
