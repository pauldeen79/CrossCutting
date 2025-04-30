namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IGenericFunction : IMember
{
    Result<object?> EvaluateGeneric<T>(FunctionCallContext context);
}
