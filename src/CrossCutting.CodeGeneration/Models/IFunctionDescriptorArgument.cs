namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionDescriptorArgument
{
    [Required] string Name { get; }
    [Required] string TypeName { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
    bool IsRequired { get; }
}
