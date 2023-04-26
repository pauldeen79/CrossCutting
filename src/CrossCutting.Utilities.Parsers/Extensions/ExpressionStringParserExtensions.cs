namespace CrossCutting.Utilities.Parsers.Extensions;

public static class ExpressionStringParserExtensions
{
    public static Result<object?> Parse(this IExpressionStringParser instance, string input, IFormatProvider formatProvider)
        => instance.Parse(input, formatProvider, null);
}
