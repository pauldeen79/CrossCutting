namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface INonGenericMember : IMember
{
    Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token);
}

public interface INonGenericMember<T> : IMember
{
    Task<Result<T>> EvaluateTypedAsync(FunctionCallContext context, CancellationToken token);
}
