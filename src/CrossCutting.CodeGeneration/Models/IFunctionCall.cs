namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionCall
{
    [Required] string Name { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionCallArgument> Arguments { get; }
}
