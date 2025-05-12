namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ExpressionEvaluatorExtensions
{
    public static Task<ExpressionParseResult> ParseAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.ParseAsync(new ExpressionEvaluatorContext(expression, settings, instance, null));

    public static Task<ExpressionParseResult> ParseAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, IReadOnlyDictionary<string, Func<Result<object?>>>? context)
        => instance.ParseAsync(new ExpressionEvaluatorContext(expression, settings, instance, context));

    public static Task<Result<object?>> EvaluateAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.EvaluateAsync(new ExpressionEvaluatorContext(expression, settings, instance, null));

    public static Task<Result<object?>> EvaluateAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, IReadOnlyDictionary<string, Func<Result<object?>>>? context)
        => instance.EvaluateAsync(new ExpressionEvaluatorContext(expression, settings, instance, context));
}
