namespace CrossCutting.Utilities.QueryEvaluator;

public partial record ExpressionBase
{
    public abstract Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
