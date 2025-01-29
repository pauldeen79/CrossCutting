namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ExpressionStringEvaluatorExtensions
{
    public static Result<object?> Evaluate(this IExpressionStringEvaluator instance, string input, ExpressionStringEvaluatorSettings settings)
        => instance.Evaluate(input, settings, null, null);

    public static Result<object?> Evaluate(this IExpressionStringEvaluator instance, string input, ExpressionStringEvaluatorSettings settings, IFormattableStringParser formattableStringParser)
        => instance.Evaluate(input, settings, null, formattableStringParser);

    public static Result<object?> Evaluate(this IExpressionStringEvaluator instance, string input, ExpressionStringEvaluatorSettings settings, object? context)
        => instance.Evaluate(input, settings, context, null);

    public static Result<Type> Validate(this IExpressionStringEvaluator instance, string input, ExpressionStringEvaluatorSettings settings)
        => instance.Validate(input, settings, null, null);

    public static Result<Type> Validate(this IExpressionStringEvaluator instance, string input, ExpressionStringEvaluatorSettings settings, IFormattableStringParser formattableStringParser)
        => instance.Validate(input, settings, null, formattableStringParser);

    public static Result<Type> Validate(this IExpressionStringEvaluator instance, string input, ExpressionStringEvaluatorSettings settings, object? context)
        => instance.Validate(input, settings, context, null);
}
