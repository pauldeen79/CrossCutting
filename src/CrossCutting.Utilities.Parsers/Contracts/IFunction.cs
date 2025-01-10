namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunction
{
    Result Validate(FunctionCallContext context);

    Result<object?> Evaluate(FunctionCallContext context);
}

public interface ITypedFunction<T> : IFunction
{
    Result<T> EvaluateTyped(FunctionCallContext context);
}
