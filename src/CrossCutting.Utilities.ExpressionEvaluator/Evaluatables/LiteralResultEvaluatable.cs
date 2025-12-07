namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record LiteralResultEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value);
}

public partial record LiteralResultEvaluatable<T>
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value.TryCastAllowNull<object?>());

    public Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value);
}
