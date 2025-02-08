namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

internal interface IFunctionArgument : IFunctionCallArgumentBase
{
    [Required][ValidateObject] IFunctionCall Function { get; }
}
