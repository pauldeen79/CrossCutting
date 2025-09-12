namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record StringStartsWithCondition
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => ConditionHelper.EvaluateStringConditionAsync(SourceExpression, CompareExpression, context, (firstString, secondString) => firstString.StartsWith(secondString, StringComparison), token);
}
