namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParseResultEvaluator
{
    Result<object?> Evaluate(FunctionParseResult functionResult, IExpressionParser parser, object? context);
}
