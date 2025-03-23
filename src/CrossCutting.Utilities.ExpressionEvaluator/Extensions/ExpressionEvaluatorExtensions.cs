namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ExpressionEvaluatorExtensions
{
    public static Result<Type> Validate(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.Validate(expression, settings, null);

    public static Result<object?> Evaluate(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.Evaluate(expression, settings, null);

    public static Result<T> EvaluateTyped<T>(this IExpressionEvaluator instance, string expression, ExpressionEvaluatorSettings settings)
        => instance.EvaluateTyped<T>(expression, settings, null);
}
