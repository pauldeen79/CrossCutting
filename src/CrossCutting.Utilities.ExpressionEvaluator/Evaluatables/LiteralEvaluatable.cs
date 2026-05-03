namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record LiteralEvaluatable : ILiteralEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success(Value));

    public object? GetValue() => Value;
}

public partial record LiteralEvaluatable<T> : ILiteralEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success<object?>(Value));

    public Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success(Value));

    public object? GetValue() => Value;
    
    IEvaluatableBuilder<T> IEvaluatable<T>.ToTypedBuilder()
        => ToTypedBuilder();
}
