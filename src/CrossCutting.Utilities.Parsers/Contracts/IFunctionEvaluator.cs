namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionEvaluator
{
    Result Validate(FunctionCall functionCall, IExpressionParser parser, object? context);

    Result<object?> Evaluate(FunctionCall functionCall, IExpressionParser parser, object? context);
}
