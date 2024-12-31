namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFormattableStringParser
{
    Result Validate(string input, FormattableStringParserSettings settings, object? context);

    Result<GenericFormattableString> Parse(string input, FormattableStringParserSettings settings, object? context);
}
