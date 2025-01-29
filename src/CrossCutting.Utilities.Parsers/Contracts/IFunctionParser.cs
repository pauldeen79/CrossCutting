namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParser
{
    Result<FunctionCall> Parse(string function, FunctionParserSettings settings, object? context);
}
