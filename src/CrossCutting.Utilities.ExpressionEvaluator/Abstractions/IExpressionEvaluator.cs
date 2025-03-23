namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionEvaluator
{
    Result<Type> Validate(string expression, ExpressionEvaluatorSettings settings, object? context);

    Result<object?> Evaluate(string expression, ExpressionEvaluatorSettings settings, object? context);

    Result<T> EvaluateTyped<T>(string expression, ExpressionEvaluatorSettings settings, object? context);
}
