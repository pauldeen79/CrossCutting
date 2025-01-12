namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParserArgumentProcessor
{
    int Order { get; }

    Result<FunctionCallArgument> Process(string argument, IReadOnlyCollection<FunctionCall> functionCalls, IFormatProvider formatProvider, IFormattableStringParser? formattableStringParser, object? context);
}
