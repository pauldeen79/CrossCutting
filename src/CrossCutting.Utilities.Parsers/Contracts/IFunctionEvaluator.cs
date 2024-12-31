namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionEvaluator
{
    Result Validate(FunctionCall functionCall, IExpressionEvaluator evaluator, object? context);

    Result<object?> Evaluate(FunctionCall functionCall, IExpressionEvaluator evaluator, object? context);
}
