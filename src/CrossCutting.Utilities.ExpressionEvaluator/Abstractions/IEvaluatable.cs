namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IEvaluatable
{
    Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
public interface IEvaluatable<T> : IEvaluatable
{
    Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
