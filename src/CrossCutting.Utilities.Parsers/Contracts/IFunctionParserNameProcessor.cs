namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParserNameProcessor
{
    Result<FunctionNameAndTypeArguments> Process(string input);
}
