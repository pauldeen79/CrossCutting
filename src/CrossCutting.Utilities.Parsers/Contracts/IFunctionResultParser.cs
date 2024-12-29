namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionResultParser
{
    Result Validate(FunctionCall functionCall, object? context, IFunctionEvaluator evaluator, IExpressionParser parser);

    Result<object?> Parse(FunctionCall functionCall, object? context, IFunctionEvaluator evaluator, IExpressionParser parser);
}
