namespace CrossCutting.CodeGeneration.Models;

internal interface IFunctionDescriptorArgument
{
    [Required] string Name { get; }
    [Required] string TypeName { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
    bool IsRequired { get; }
}
