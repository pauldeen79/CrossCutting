namespace CrossCutting.Utilities.Parsers;

public class FunctionParserSettings
{
    public FunctionParserSettings(IFormatProvider formatProvider, IFormattableStringParser? formattableStringParser)
    {
        ArgumentGuard.IsNotNull(formatProvider, nameof(formatProvider));

        FormatProvider = formatProvider;
        FormattableStringParser = formattableStringParser;
    }

    public IFormatProvider FormatProvider { get; }
    public IFormattableStringParser? FormattableStringParser { get; }
}
