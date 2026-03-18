namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record UnaryNegateOperatorEvaluatable : IChildEvaluatablesContainer
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await Operand.EvaluateAsync(context, token).ConfigureAwait(false))
            .TryCast<bool>("Expression is not of type boolean")
            .OnSuccess<bool, object?>(value => !value);

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await Operand.EvaluateAsync(context, token).ConfigureAwait(false))
            .TryCast<bool>("Expression is not of type boolean")
            .OnSuccess(result => Result.Success(!result.Value));

    public IEnumerable<IEvaluatable> GetChildEvaluatables() => [Operand];
}