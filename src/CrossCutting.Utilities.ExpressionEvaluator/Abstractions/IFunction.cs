namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunction : IMember
{
    Result<object?> Evaluate(FunctionCallContext context);
}

public interface IFunction<T> : IFunction
{
    Result<T> EvaluateTyped(FunctionCallContext context);
}
