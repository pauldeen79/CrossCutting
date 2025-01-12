namespace CrossCutting.Utilities.Parsers.Extensions;

public static class MathematicExpressionEvaluatorExtensions
{
    public static Result<object?> Evaluate(this IMathematicExpressionEvaluator instance, string input, IFormatProvider formatProvider)
        => instance.Evaluate(input, formatProvider, null);

    public static Result Validate(this IMathematicExpressionEvaluator instance, string input, IFormatProvider formatProvider)
        => instance.Validate(input, formatProvider, null);
}
