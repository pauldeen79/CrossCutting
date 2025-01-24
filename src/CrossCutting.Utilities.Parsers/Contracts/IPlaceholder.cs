namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IPlaceholder
{
    // Note that the return type is always FormattableString
    Result Validate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser);

    Result<GenericFormattableString> Evaluate(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser);
}
