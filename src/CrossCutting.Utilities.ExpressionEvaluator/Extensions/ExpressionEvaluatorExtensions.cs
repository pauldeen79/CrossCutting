namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ExpressionEvaluatorExtensions
{
    public static ExpressionParseResult Parse(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.Parse(new ExpressionEvaluatorContext(expression, settings, instance, null));

    public static ExpressionParseResult Parse(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, IReadOnlyDictionary<string, Func<Result<object?>>>? context)
        => instance.Parse(new ExpressionEvaluatorContext(expression, settings, instance, context));

    public static Result<object?> Evaluate(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.Evaluate(new ExpressionEvaluatorContext(expression, settings, instance, null));

    public static Result<object?> Evaluate(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, IReadOnlyDictionary<string, Func<Result<object?>>>? context)
        => instance.Evaluate(new ExpressionEvaluatorContext(expression, settings, instance, context));

    public static Result<T> EvaluateTyped<T>(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.EvaluateTyped<T>(new ExpressionEvaluatorContext(expression, settings, instance, null));

    public static Result<T> EvaluateTyped<T>(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, IReadOnlyDictionary<string, Func<Result<object?>>>? context)
        => instance.EvaluateTyped<T>(new ExpressionEvaluatorContext(expression, settings, instance, context));
}
