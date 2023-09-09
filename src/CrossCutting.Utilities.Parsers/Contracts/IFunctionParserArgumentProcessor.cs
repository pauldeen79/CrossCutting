namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParserArgumentProcessor
{
    int Order { get; }
    Result<FunctionParseResultArgument> Process(string stringArgument, IReadOnlyCollection<FunctionParseResult> results, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);
}
