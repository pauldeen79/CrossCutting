namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

internal interface IRecursiveArgument : IFunctionCallArgument
{
    [Required][ValidateObject] IFunctionCall Function { get; }
}
