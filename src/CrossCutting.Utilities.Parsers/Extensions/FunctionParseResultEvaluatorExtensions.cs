namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionParseResultEvaluatorExtensions
{
    public static Result<object?> Evaluate(this IFunctionParseResultEvaluator instance, FunctionParseResult functionResult, IExpressionParser parser, IFormatProvider formatProvider)
        => instance.Evaluate(functionResult, parser, formatProvider, null);
}
