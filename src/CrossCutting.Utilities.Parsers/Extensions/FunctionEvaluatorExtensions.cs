namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionEvaluatorExtensions
{
    public static Result<Type> Validate(this IFunctionEvaluator instance, FunctionCall functionCall)
        => instance.Validate(functionCall, CultureInfo.InvariantCulture, null);

    public static Result<Type> Validate(this IFunctionEvaluator instance, FunctionCall functionCall, IFormatProvider formatProvider)
        => instance.Validate(functionCall, formatProvider, null);

    public static Result<Type> Validate(this IFunctionEvaluator instance, FunctionCall functionCall, object? context)
        => instance.Validate(functionCall, CultureInfo.InvariantCulture, context);

    public static Result<object?> Evaluate(this IFunctionEvaluator instance, FunctionCall functionCall)
        => instance.Evaluate(functionCall, CultureInfo.InvariantCulture, null);

    public static Result<object?> Evaluate(this IFunctionEvaluator instance, FunctionCall functionCall, IFormatProvider formatProvider)
        => instance.Evaluate(functionCall, formatProvider, null);

    public static Result<object?> Evaluate(this IFunctionEvaluator instance, FunctionCall functionCall, object? context)
        => instance.Evaluate(functionCall, CultureInfo.InvariantCulture, context);

    public static Result<T> EvaluateTyped<T>(this IFunctionEvaluator instance, FunctionCall functionCall)
        => instance.EvaluateTyped<T>(functionCall, CultureInfo.InvariantCulture, null);

    public static Result<T> EvaluateTyped<T>(this IFunctionEvaluator instance, FunctionCall functionCall, IFormatProvider formatProvider)
        => instance.EvaluateTyped<T>(functionCall, formatProvider, null);

    public static Result<T> EvaluateTyped<T>(this IFunctionEvaluator instance, FunctionCall functionCall, object? context)
        => instance.EvaluateTyped<T>(functionCall, CultureInfo.InvariantCulture, context);
}
