namespace CrossCutting.CodeGeneration.Models;

internal interface IFunctionCall
{
    [Required] string Name { get; }
    [Required][ValidateObject] IReadOnlyCollection<Abstractions.IFunctionCallArgument> Arguments { get; }
}
