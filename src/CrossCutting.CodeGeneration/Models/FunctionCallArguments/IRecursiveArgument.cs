namespace CrossCutting.CodeGeneration.Models.FunctionCallArguments;

public interface IRecursiveArgument : IFunctionCallArgument
{
    [Required][ValidateObject] IFunctionCall Function { get; }
}
