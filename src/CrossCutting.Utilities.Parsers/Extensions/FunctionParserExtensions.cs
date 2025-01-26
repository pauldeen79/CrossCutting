namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionParserExtensions
{
    public static Result<FunctionCall> Parse(this IFunctionParser instance, string input, IFormatProvider formatProvider)
        => instance.Parse(input, new FunctionParserSettings(formatProvider, null), null);

    public static Result<FunctionCall> Parse(this IFunctionParser instance, string input, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser)
        => instance.Parse(input, new FunctionParserSettings(formatProvider, formattableStringParser), null);

    public static Result<FunctionCall> Parse(this IFunctionParser instance, string input, IFormatProvider formatProvider, object? context)
        => instance.Parse(input, new FunctionParserSettings(formatProvider, null), context);
}
