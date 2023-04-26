namespace CrossCutting.Utilities.Parsers.PlaceholderProcessors;

public class UnknownPlaceholderProcessor : IPlaceholderProcessor
{
    public int Order => 1000;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context)
        => Result<string>.NotSupported($"Unknown placeholder in value: {value}");
}
