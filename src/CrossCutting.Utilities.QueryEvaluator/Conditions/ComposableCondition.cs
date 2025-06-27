namespace CrossCutting.Utilities.QueryEvaluator.Conditions;

public partial record ComposableCondition : IEvaluatable<bool>
{
    public async Task<Result<object?>> EvaluateAsync(CancellationToken token)
        => await EvaluateTypedAsync(token).ConfigureAwait(false);

    public Task<Result<bool>> EvaluateTypedAsync(CancellationToken token)
        => Task.Run(() => Operator.Evaluate(LeftExpression, RightExpression, StringComparison), token);
}
