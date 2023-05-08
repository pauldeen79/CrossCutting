namespace CrossCutting.CodeGeneration.Models.FunctionParseResultArguments;

public interface ILiteralArgument : IFunctionParseResultArgument
{
    string Value { get; }
}
