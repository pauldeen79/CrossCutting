namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionEvaluator
{
    Result<Type> Validate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context);

    Result<object?> Evaluate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context);

    Result<T> EvaluateTyped<T>(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider, object? context);
}
