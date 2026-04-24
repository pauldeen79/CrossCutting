namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record StringEndsWithOperatorEvaluatable : IChildEvaluatablesContainer
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await EvaluateTypedAsync(context, token).ConfigureAwait(false)).TryCast<object?>();

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(LeftOperand), async () => (await LeftOperand.EvaluateAsync(context, token).ConfigureAwait(false)).TryCast<string>("LeftOperand is not of type string"))
            .Add(nameof(RightOperand), async () => (await RightOperand.EvaluateAsync(context, token).ConfigureAwait(false)).TryCast<string>("RightOperand is not of type string"))
            .BuildAsync().ConfigureAwait(false))
            .OnSuccess(results => results.GetValue<string>(nameof(LeftOperand)).EndsWith(results.GetValue<string>(nameof(RightOperand)), StringComparison));

    public IEnumerable<IEvaluatable> GetChildEvaluatables() =>
    [
        LeftOperand,
        RightOperand
    ];
}