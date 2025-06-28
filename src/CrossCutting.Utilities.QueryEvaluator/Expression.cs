namespace CrossCutting.Utilities.QueryEvaluator;

public partial record Expression
{
    public abstract Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
