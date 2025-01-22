namespace CrossCutting.Utilities.Parsers.Contracts;

public interface ITypedFunction<T> : IFunction
{
    Result<T> EvaluateTyped(FunctionCallContext context);
}
