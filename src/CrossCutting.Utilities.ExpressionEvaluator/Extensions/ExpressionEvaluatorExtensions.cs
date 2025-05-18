namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ExpressionEvaluatorExtensions
{
    public static Task<ExpressionParseResult> ParseAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.ParseAsync(expression, settings, CancellationToken.None);

    public static Task<ExpressionParseResult> ParseAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, CancellationToken token)
        => instance.ParseAsync(new ExpressionEvaluatorContext(expression, settings, instance, null), token);

    public static Task<ExpressionParseResult> ParseAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, IReadOnlyDictionary<string, Task<Result<object?>>>? state)
        => instance.ParseAsync(expression, settings, CancellationToken.None);

    public static Task<ExpressionParseResult> ParseAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, IReadOnlyDictionary<string, Task<Result<object?>>>? state, CancellationToken token)
        => instance.ParseAsync(new ExpressionEvaluatorContext(expression, settings, instance, state), token);

    public static Task<Result<object?>> EvaluateAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, CancellationToken token)
        => instance.EvaluateAsync(new ExpressionEvaluatorContext(expression, settings, instance, null), token);

    public static Task<Result<object?>> EvaluateAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.EvaluateAsync(expression, settings, CancellationToken.None);

    public static Task<Result<object?>> EvaluateAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, IReadOnlyDictionary<string, Task<Result<object?>>>? state, CancellationToken token)
        => instance.EvaluateAsync(new ExpressionEvaluatorContext(expression, settings, instance, state), token);

    public static Task<Result<object?>> EvaluateAsync(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings, IReadOnlyDictionary<string, Task<Result<object?>>>? state)
        => instance.EvaluateAsync(expression, settings, state, CancellationToken.None);
}
