namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParserArgumentProcessor
{
    Result<FunctionCallArgument> Process(string argument, IReadOnlyCollection<FunctionCall> functionCalls, FunctionParserSettings settings, object? context);
}
