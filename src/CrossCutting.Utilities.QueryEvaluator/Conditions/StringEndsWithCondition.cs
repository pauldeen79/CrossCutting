namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record StringEndsWithCondition
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => ConditionHelper.EvaluateStringConditionAsync(FirstExpression, SecondExpression, context, (firstString, secondString) => firstString.EndsWith(secondString, StringComparison), token);
}
