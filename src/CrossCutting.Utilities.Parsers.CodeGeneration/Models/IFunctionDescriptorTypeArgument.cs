namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models;

internal interface IFunctionDescriptorTypeArgument
{
    [Required] string Name { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
}
