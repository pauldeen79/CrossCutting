namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParserArgumentProcessor
{
    int Order { get; }

    Result Validate(string stringArgument, IReadOnlyCollection<FunctionCall> results, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);

    Result<FunctionCallArgument> Process(string stringArgument, IReadOnlyCollection<FunctionCall> results, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);
}
