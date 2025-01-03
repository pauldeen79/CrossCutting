namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFormattableStringParser
{
    Result Validate(string format, FormattableStringParserSettings settings, object? context);

    Result<GenericFormattableString> Parse(string format, FormattableStringParserSettings settings, object? context);
}
