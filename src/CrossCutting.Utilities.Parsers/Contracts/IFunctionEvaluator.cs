namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionEvaluator
{
    Result<object?> Evaluate(FunctionCall functionResult, IExpressionParser parser, object? context);
}
