namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record LiteralResultEvaluatable : ILiteralResultEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value);

    public Result GetValue() => Value;
}

public partial record LiteralResultEvaluatable<T> : ILiteralResultEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value.TryCastAllowNull<object?>());

    public Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value);

    public Result GetValue() => Value;

    IEvaluatableBuilder<T> IEvaluatable<T>.ToTypedBuilder()
        => ToTypedBuilder();
}
