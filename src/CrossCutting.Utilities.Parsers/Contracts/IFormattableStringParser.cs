namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFormattableStringParser
{
    Result<FormattableStringParserResult> Parse(string input, IFormatProvider formatProvider, object? context);
}
