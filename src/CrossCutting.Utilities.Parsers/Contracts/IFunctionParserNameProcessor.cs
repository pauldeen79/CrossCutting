namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParserNameProcessor
{
    Result<string> Process(string input);
}
