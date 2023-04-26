namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FunctionParserExtensions
{
    public static Result<FunctionParseResult> Parse(this IFunctionParser instance, string input, IFormatProvider formatProvider) => instance.Parse(input, formatProvider, null);
}
