namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionResultParser
{
    Result<object?> Parse(FunctionCall functionCall, object? context, IFunctionEvaluator evaluator, IExpressionParser parser);
}
