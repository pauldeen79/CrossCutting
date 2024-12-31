namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ExpressionEvaluatorExtensions
{
    public static Result<object?> Evaluate(this IExpressionEvaluator instance, string value, IFormatProvider formatProvider)
        => instance.Evaluate(value, formatProvider, null);

    public static Result Validate(this IExpressionEvaluator instance, string value, IFormatProvider formatProvider)
        => instance.Validate(value, formatProvider, null);
}
