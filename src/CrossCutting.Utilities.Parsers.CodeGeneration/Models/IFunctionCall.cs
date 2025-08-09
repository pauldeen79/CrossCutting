namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models;

internal interface IFunctionCall
{
    [Required] string Name { get; }
    [Required][ValidateObject] IReadOnlyCollection<Abstractions.IFunctionCallArgument> Arguments { get; }
    [Required] IReadOnlyCollection<Abstractions.IFunctionCallTypeArgument> TypeArguments { get; }
}
