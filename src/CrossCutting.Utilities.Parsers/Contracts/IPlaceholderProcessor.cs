namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IPlaceholderProcessor
{
    int Order { get; }

    Result<FormattableStringParserResult> Validate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser);

    Result<FormattableStringParserResult> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser);
}
