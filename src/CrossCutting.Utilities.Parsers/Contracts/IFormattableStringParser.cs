namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFormattableStringParser
{
    // Note that the return type is always string
    Result Validate(string format, FormattableStringParserSettings settings, object? context);

    Result<GenericFormattableString> Parse(string format, FormattableStringParserSettings settings, object? context);
}
