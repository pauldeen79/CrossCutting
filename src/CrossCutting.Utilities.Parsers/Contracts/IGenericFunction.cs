namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IGenericFunction : IFunction
{
    Result<object?> EvaluateGeneric<T>(FunctionCallContext context);
}
