namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IPlaceholderProcessor
{
    Result<string> Process(string value, IFormatProvider formatProvider, object? context);
}
