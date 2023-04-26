namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParserArgumentProcessor
{
    int Order { get; }
    Result<FunctionParseResultArgument> Process(string stringArgument/*, IReadOnlyCollection<FunctionParseResultArgument> currentArguments*/, IReadOnlyCollection<FunctionParseResult> results);
}
