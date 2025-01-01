namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionEvaluator
{
    Result Validate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, object? context);

    Result<object?> Evaluate(FunctionCall functionCall, IExpressionEvaluator expressionEvaluator, object? context);
}
