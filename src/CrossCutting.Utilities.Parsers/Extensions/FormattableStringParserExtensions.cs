namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FormattableStringParserExtensions
{
    public static Result<FormattableStringParserResult> Parse(this IFormattableStringParser instance, string input, FormattableStringParserSettings settings)
        => instance.Parse(input, settings, null);

    public static Result<FormattableStringParserResult> Parse(this IFormattableStringParser instance, string input, IFormatProvider formatProvider)
        => instance.Parse(input, new FormattableStringParserSettingsBuilder().WithFormatProvider(formatProvider).Build(), null);
}
