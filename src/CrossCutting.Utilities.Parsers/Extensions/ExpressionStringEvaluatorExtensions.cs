namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ExpressionStringEvaluatorExtensions
{
    public static Result<object?> Evaluate(this IExpressionStringEvaluator instance, string input, IFormatProvider formatProvider)
        => instance.Evaluate(input, formatProvider, null, null);

    public static Result<object?> Evaluate(this IExpressionStringEvaluator instance, string input, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser)
        => instance.Evaluate(input, formatProvider, null, formattableStringParser);

    public static Result<object?> Evaluate(this IExpressionStringEvaluator instance, string input, IFormatProvider formatProvider, object? context)
        => instance.Evaluate(input, formatProvider, context, null);

    public static Result<Type> Validate(this IExpressionStringEvaluator instance, string input, IFormatProvider formatProvider)
        => instance.Validate(input, formatProvider, null, null);

    public static Result<Type> Validate(this IExpressionStringEvaluator instance, string input, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser)
        => instance.Validate(input, formatProvider, null, formattableStringParser);

    public static Result<Type> Validate(this IExpressionStringEvaluator instance, string input, IFormatProvider formatProvider, object? context)
        => instance.Validate(input, formatProvider, context, null);
}
