namespace CrossCutting.Utilities.Parsers.Extensions;

public static class StringExtensions
{
    public static FormattableStringParserResult ToFormattableStringParserResult(this string instance)
        => new FormattableStringParserResult(instance);
}
