namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunction
{
    Result Validate(FunctionCall functionCall, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator);

    Result<object?> Evaluate(FunctionCall functionCall, object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator);
}
