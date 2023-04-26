namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FormattableStringParserExtensions
{
    public static Result<string> Parse(this IFormattableStringParser instance, string input, IFormatProvider formatProvider)
        => instance.Parse(input, formatProvider, null);
}
