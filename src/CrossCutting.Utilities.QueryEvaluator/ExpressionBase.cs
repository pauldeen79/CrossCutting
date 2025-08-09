namespace CrossCutting.Utilities.QueryEvaluator.Core;

public partial record ExpressionBase
{
    public abstract Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
