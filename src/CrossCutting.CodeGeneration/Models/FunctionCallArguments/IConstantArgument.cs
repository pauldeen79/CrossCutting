namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

internal interface IConstantArgument : IFunctionCallArgument
{
    string Value { get; }
}
