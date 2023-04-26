namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParserNameProcessor
{
    int Order { get; }
    Result<string> Process(string input);
}
