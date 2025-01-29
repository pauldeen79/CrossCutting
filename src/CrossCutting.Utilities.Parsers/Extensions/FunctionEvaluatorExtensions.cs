namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionEvaluatorExtensions
{
    public static Result<Type> Validate(this IFunctionEvaluator instance, FunctionCall functionCall, FunctionEvaluatorSettings settings)
        => instance.Validate(functionCall, settings, null);

    public static Result<object?> Evaluate(this IFunctionEvaluator instance, FunctionCall functionCall, FunctionEvaluatorSettings settings)
        => instance.Evaluate(functionCall, settings, null);

    public static Result<T> EvaluateTyped<T>(this IFunctionEvaluator instance, FunctionCall functionCall, FunctionEvaluatorSettings settings)
        => instance.EvaluateTyped<T>(functionCall, settings, null);
}
