namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record DelegateResultEvaluatable : IDelegateResultEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value());

    public Func<Result<object?>> GetValue() => Value;
}

public partial record DelegateResultEvaluatable<T> : IDelegateResultEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value().TryCastAllowNull<object?>());

    public Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value());

    public Func<Result<object?>> GetValue() => () => Value.Invoke().TryCastAllowNull<object?>();

    IEvaluatableBuilder<T> IEvaluatable<T>.ToTypedBuilder()
        => ToTypedBuilder();
}