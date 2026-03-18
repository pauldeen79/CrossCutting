namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record NullOperatorEvaluatable : IChildEvaluatablesContainer
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await EvaluateTypedAsync(context, token).ConfigureAwait(false)).TryCast<object?>();

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(Operand), () => Operand.EvaluateAsync(context, token))
            .BuildAsync().ConfigureAwait(false))
            .OnSuccess(results => results.GetValue(nameof(Operand)) is null);

    public IEnumerable<IEvaluatable> GetChildEvaluatables() => [Operand];
}
