namespace CrossCutting.Utilities.ExpressionEvaluator;

public interface IEvaluatable
{
    Task<Result<object?>> EvaluateAsync(CancellationToken token);
}
public interface IEvaluatable<T> : IEvaluatable
{
    Task<Result<T>> EvaluateTypedAsync(CancellationToken token);
}
