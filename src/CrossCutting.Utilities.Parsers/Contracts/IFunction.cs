namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunction
{
    Result Validate(FunctionCall functionCall, object? context, IFunctionEvaluator evaluator, IExpressionEvaluator parser);

    Result<object?> Evaluate(FunctionCall functionCall, object? context, IFunctionEvaluator evaluator, IExpressionEvaluator parser);
}
