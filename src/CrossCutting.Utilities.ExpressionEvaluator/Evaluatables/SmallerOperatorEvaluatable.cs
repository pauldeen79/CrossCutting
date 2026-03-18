namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record SmallerOperatorEvaluatable : IChildEvaluatablesContainer
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await EvaluateTypedAsync(context, token).ConfigureAwait(false)).TryCast<object?>();

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(LeftOperand), () => LeftOperand.EvaluateAsync(context, token))
            .Add(nameof(RightOperand), () => RightOperand.EvaluateAsync(context, token))
            .BuildAsync().ConfigureAwait(false))
            .OnSuccess(results => SmallerThan.Evaluate(results.GetValue(nameof(LeftOperand)), results.GetValue(nameof(RightOperand))));

    public IEnumerable<IEvaluatable> GetChildEvaluatables() =>
    [
        LeftOperand,
        RightOperand
    ];
}
