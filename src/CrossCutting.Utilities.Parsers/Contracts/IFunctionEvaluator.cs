namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionEvaluator
{
    Result<object?> Evaluate(FunctionCall functionCall, IExpressionParser parser, object? context);
}
