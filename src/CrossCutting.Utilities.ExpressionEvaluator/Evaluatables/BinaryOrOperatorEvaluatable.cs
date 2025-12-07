namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record BinaryOrOperatorEvaluatable
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await EvaluateTypedAsync(context, token).ConfigureAwait(false)).TryCast<object?>();

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(LeftOperand), async () => (await LeftOperand.EvaluateAsync(context, token).ConfigureAwait(false)).TryCast<bool>())
            .Add(nameof(RightOperand), async () => (await RightOperand.EvaluateAsync(context, token).ConfigureAwait(false)).TryCast<bool>())
            .BuildAsync().ConfigureAwait(false))
            .OnSuccess(results => EvaluateOr(results.GetValue<bool>(nameof(LeftOperand)), results.GetValue<bool>(nameof(RightOperand))));

    private static Result<bool> EvaluateOr(bool left, bool right)
        => Result.Success(left || right);
}
