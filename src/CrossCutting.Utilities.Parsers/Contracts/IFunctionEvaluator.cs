namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionEvaluator
{
    Result<Type> Validate(FunctionCall functionCall, IFormatProvider formatProvider, object? context);

    Result<object?> Evaluate(FunctionCall functionCall, IFormatProvider formatProvider, object? context);

    Result<T> EvaluateTyped<T>(FunctionCall functionCall, IFormatProvider formatProvider, object? context);
}
