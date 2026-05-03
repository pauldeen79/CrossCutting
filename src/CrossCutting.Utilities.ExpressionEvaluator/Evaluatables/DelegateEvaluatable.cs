namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record DelegateEvaluatable : IDelegateEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success(Value()));

    public Func<object?> GetValue() => Value;
}

public partial record DelegateEvaluatable<T> : IDelegateEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success<object?>(Value()));

    public Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success(Value()));

    public Func<object?> GetValue() => () => Value();

    IEvaluatableBuilder<T> IEvaluatable<T>.ToTypedBuilder()
        => ToTypedBuilder();
}