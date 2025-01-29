namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IPlaceholder
{
    // Note that the return type is always FormattableString
    Result Validate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser);

    Result<GenericFormattableString> Evaluate(string value, PlaceholderSettings settings, object? context, IFormattableStringParser formattableStringParser);
}
