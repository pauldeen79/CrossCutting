namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParser
{
    Result<object> Parse(FunctionParseResult functionParseResult);
}
