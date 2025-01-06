namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

internal interface IDynamicArgument : IFunctionCallArgument
{
    [Required][ValidateObject] IFunctionCall Function { get; }
}
