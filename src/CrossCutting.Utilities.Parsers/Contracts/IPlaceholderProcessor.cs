namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IPlaceholderProcessor
{
    int Order { get; }
    Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser);
}
