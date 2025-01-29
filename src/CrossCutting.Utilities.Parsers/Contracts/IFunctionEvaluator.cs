namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionEvaluator
{
    Result<Type> Validate(FunctionCall functionCall, FunctionEvaluatorSettings settings, object? context);

    Result<object?> Evaluate(FunctionCall functionCall, FunctionEvaluatorSettings settings, object? context);

    Result<T> EvaluateTyped<T>(FunctionCall functionCall, FunctionEvaluatorSettings settings, object? context);
}
