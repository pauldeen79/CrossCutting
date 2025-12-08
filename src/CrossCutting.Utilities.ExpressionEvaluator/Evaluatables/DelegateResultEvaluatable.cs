namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record DelegateResultEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value());
}

public partial record DelegateResultEvaluatable<T>
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value().TryCastAllowNull<object?>());

    public Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Value());
}