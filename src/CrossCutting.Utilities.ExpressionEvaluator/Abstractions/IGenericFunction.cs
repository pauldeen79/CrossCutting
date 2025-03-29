namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IGenericFunction
{
    Result<object?> EvaluateGeneric<T>(FunctionCallContext context);
}
