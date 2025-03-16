namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ExpressionEvaluatorExtensions
{
    public static Result<object?> Evaluate(this IExpressionEvaluator instance, string value, IFormatProvider formatProvider)
        => instance.Evaluate(value, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(formatProvider), null);

    public static Result<Type> Validate(this IExpressionEvaluator instance, string value, IFormatProvider formatProvider)
        => instance.Validate(value, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(formatProvider), null);

    public static Result<object?> Evaluate(this IExpressionEvaluator instance, string value, ExpressionEvaluatorSettings settings)
    => instance.Evaluate(value, settings, null);

    public static Result<Type> Validate(this IExpressionEvaluator instance, string value, ExpressionEvaluatorSettings settings)
        => instance.Validate(value, settings, null);
}
