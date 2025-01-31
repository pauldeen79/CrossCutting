namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParserArgumentProcessor
{
    Result<IFunctionCallArgument> Process(string argument, IReadOnlyCollection<FunctionCall> functionCalls, FunctionParserSettings settings, object? context);
}
