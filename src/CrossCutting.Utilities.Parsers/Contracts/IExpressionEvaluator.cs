namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionEvaluator
{
    Result<Type> Validate(string expression, ExpressionEvaluatorSettings settings, object? context);

    Result<object?> Evaluate(string expression, ExpressionEvaluatorSettings settings, object? context);
}
