namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunction
{
    Result<object?> Evaluate(FunctionCallContext context);
}

public interface IValidatableFunction : IFunction
{
    Result<Type> Validate(FunctionCallContext context);
}

public interface ITypedFunction<T> : IFunction
{
    Result<T> EvaluateTyped(FunctionCallContext context);
}
