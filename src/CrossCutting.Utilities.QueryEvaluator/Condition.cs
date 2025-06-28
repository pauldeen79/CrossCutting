namespace CrossCutting.Utilities.QueryEvaluator;

public partial record Condition : IEvaluatable<bool>
{
    public abstract Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
    public abstract Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
