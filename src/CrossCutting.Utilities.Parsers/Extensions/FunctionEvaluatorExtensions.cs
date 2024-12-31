namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionEvaluatorExtensions
{
    public static Result Validate(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator)
        => instance.Validate(functionCall, evaluator, null);

    public static Result<object?> Evaluate(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator)
        => instance.Evaluate(functionCall, evaluator, null);
}
