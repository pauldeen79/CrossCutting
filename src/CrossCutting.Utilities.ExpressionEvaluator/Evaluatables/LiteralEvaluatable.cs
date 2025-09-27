namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record LiteralEvaluatable
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.FromResult(Result.Success(Value));
}
