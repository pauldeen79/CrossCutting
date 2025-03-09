namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IGenericFunction
{
    Result<object?> EvaluateGeneric<T>(FunctionCallContext context);
}
