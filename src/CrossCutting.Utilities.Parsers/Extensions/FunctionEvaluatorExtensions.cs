namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionEvaluatorExtensions
{
    public static Result<Type> Validate(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator)
        => instance.Validate(functionCall, evaluator, CultureInfo.InvariantCulture, null);

    public static Result<Type> Validate(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator, IFormatProvider formatProvider)
        => instance.Validate(functionCall, evaluator, formatProvider, null);

    public static Result<Type> Validate(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator, object? context)
        => instance.Validate(functionCall, evaluator, CultureInfo.InvariantCulture, context);

    public static Result<object?> Evaluate(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator)
        => instance.Evaluate(functionCall, evaluator, CultureInfo.InvariantCulture, null);

    public static Result<object?> Evaluate(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator, IFormatProvider formatProvider)
        => instance.Evaluate(functionCall, evaluator, formatProvider, null);

    public static Result<object?> Evaluate(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator, object? context)
        => instance.Evaluate(functionCall, evaluator, CultureInfo.InvariantCulture, context);

    public static Result<T> EvaluateTyped<T>(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator)
        => instance.EvaluateTyped<T>(functionCall, evaluator, CultureInfo.InvariantCulture, null);

    public static Result<T> EvaluateTyped<T>(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator, IFormatProvider formatProvider)
        => instance.EvaluateTyped<T>(functionCall, evaluator, formatProvider, null);

    public static Result<T> EvaluateTyped<T>(this IFunctionEvaluator instance, FunctionCall functionCall, IExpressionEvaluator evaluator, object? context)
        => instance.EvaluateTyped<T>(functionCall, evaluator, CultureInfo.InvariantCulture, context);
}
