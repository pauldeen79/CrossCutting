namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

internal interface ILiteralArgument : IFunctionCallArgument
{
    string Value { get; }
}
