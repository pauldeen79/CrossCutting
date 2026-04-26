namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record EmptyEvaluatable : ILiteralEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NoContent<object?>());

    public object? GetValue() => default;
}

public partial record EmptyEvaluatable<T> : ILiteralEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NoContent<object?>());    

    public Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NoContent<T>());

    public object? GetValue() => default;

    IEvaluatableBuilder<T> IEvaluatable<T>.ToTypedBuilder()
        => ToTypedBuilder();
}