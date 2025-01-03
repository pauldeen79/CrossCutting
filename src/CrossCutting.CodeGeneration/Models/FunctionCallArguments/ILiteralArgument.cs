namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

public interface ILiteralArgument : IFunctionCallArgument
{
    string Value { get; }
}
