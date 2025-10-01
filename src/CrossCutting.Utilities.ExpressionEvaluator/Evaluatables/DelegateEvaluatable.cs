namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record DelegateEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success(Value()));
}
