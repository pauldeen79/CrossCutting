namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record EmptyEvaluatable : IEmptyEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NoContent<object?>());
}

public partial record EmptyEvaluatable<T> : IEmptyEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NoContent<object?>());    

    public Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NoContent<T>());

    IEvaluatableBuilder<T> IEvaluatable<T>.ToTypedBuilder()
        => ToTypedBuilder();
}