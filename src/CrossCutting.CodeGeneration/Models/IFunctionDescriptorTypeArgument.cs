namespace CrossCutting.CodeGeneration.Models;

internal interface IFunctionDescriptorTypeArgument
{
    [Required] string Name { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
}
