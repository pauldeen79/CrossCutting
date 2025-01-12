namespace CrossCutting.Utilities.Parsers.Extensions;

public static class FormattableStringParserExtensions
{
    public static Result<GenericFormattableString> Parse(this IFormattableStringParser instance, string input, FormattableStringParserSettings settings)
        => instance.Parse(input, settings, null);

    public static Result<GenericFormattableString> Parse(this IFormattableStringParser instance, string input, IFormatProvider formatProvider)
        => instance.Parse(input, new FormattableStringParserSettingsBuilder().WithFormatProvider(formatProvider).Build(), null);

    public static Result<GenericFormattableString> Parse(this IFormattableStringParser instance, string input, IFormatProvider formatProvider, object? context)
        => instance.Parse(input, new FormattableStringParserSettingsBuilder().WithFormatProvider(formatProvider).Build(), context);

    public static Result Validate(this IFormattableStringParser instance, string input, FormattableStringParserSettings settings)
        => instance.Validate(input, settings, null);

    public static Result Validate(this IFormattableStringParser instance, string input, IFormatProvider formatProvider)
        => instance.Validate(input, new FormattableStringParserSettingsBuilder().WithFormatProvider(formatProvider).Build(), null);

    public static Result Validate(this IFormattableStringParser instance, string input, IFormatProvider formatProvider, object? context)
        => instance.Validate(input, new FormattableStringParserSettingsBuilder().WithFormatProvider(formatProvider).Build(), context);
}
