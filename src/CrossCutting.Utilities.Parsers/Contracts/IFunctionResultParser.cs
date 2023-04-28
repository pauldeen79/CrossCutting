namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionResultParser
{
    Result<object?> Parse(FunctionParseResult functionParseResult, object? context, IFunctionParseResultEvaluator evaluator);
}
