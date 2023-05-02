namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParseResultEvaluator
{
    Result<object?> Evaluate(FunctionParseResult functionResult, IExpressionParser parser, IFormatProvider formatProvider, object? context);
}
