namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IGenericFunction : IMember
{
    Task<Result<object?>> EvaluateGenericAsync<T>(FunctionCallContext context);
}
