namespace CrossCutting.Utilities.ExpressionEvaluator;

public interface IEvaluatable
{
    Task<Result<object?>> EvaluateAsync();
}
public interface IEvaluatable<T> : IEvaluatable
{
    Task<Result<T>> EvaluateTypedAsync();
}
