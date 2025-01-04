namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionDescriptor
{
    [Required] string Name { get; }
    [Required] string TypeName { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionDescriptorArgument> Arguments { get; }
}
