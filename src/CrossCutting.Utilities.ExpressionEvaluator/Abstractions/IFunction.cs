namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunction : IMember
{
}

public interface IFunction<T> : IFunction
{
    Result<T> EvaluateTyped(FunctionCallContext context);
}
