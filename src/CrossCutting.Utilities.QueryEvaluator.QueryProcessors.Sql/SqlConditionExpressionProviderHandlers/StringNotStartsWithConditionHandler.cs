namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class StringNotStartsWithConditionHandler : ConditionExpressionHandlerBase<StringNotStartsWithCondition>
{
    protected override Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<StringNotStartsWithCondition> context, CancellationToken token)
        => GetStringConditionExpressionAsync(context, new StringConditionParameters("NOT LIKE", "{0}%"), token);
}
