namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ExpressionEvaluatorExtensions
{
    public static Result<Type> Validate(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.Validate(new ExpressionEvaluatorContext(expression, settings, null, instance));

    public static Result<Type> Validate(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, object? context)
        => instance.Validate(new ExpressionEvaluatorContext(expression, settings, context, instance));

    public static Result<object?> Evaluate(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.Evaluate(new ExpressionEvaluatorContext(expression, settings, null, instance));

    public static Result<object?> Evaluate(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, object? context)
        => instance.Evaluate(new ExpressionEvaluatorContext(expression, settings, context, instance));

    public static Result<T> EvaluateTyped<T>(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.EvaluateTyped<T>(new ExpressionEvaluatorContext(expression, settings, null, instance));

    public static Result<T> EvaluateTyped<T>(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, object? context)
        => instance.EvaluateTyped<T>(new ExpressionEvaluatorContext(expression, settings, context, instance));
}
