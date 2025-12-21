namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record SmallerOrEqualOperatorEvaluatable
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await EvaluateTypedAsync(context, token).ConfigureAwait(false)).TryCast<object?>();

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(LeftOperand), () => LeftOperand.EvaluateAsync(context, token))
            .Add(nameof(RightOperand), () => RightOperand.EvaluateAsync(context, token))
            .BuildAsync().ConfigureAwait(false))
            .OnSuccess(results => SmallerOrEqualThan.Evaluate(results.GetValue(nameof(LeftOperand)), results.GetValue(nameof(RightOperand))));
}
