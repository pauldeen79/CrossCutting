namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFormattableStringParser
{
    Result Validate(string input, FormattableStringParserSettings settings, object? context);

    Result<FormattableStringParserResult> Parse(string input, FormattableStringParserSettings settings, object? context);
}
