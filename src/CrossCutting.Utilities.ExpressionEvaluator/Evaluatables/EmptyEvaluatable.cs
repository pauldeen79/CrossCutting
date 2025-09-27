namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record EmptyEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.NoContent<object?>());
}
