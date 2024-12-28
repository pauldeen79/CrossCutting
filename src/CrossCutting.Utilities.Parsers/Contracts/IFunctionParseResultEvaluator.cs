namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParseResultEvaluator
{
    Result<object?> Evaluate(FunctionCall functionResult, IExpressionParser parser, object? context);
}
