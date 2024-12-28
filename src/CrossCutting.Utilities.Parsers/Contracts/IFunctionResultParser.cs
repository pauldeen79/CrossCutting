namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionResultParser
{
    Result<object?> Parse(FunctionCall functionParseResult, object? context, IFunctionEvaluator evaluator, IExpressionParser parser);
}
