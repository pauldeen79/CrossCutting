namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionDescriptor
{
    [Required] string FunctionName { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionDescriptorArgument> Arguments { get; }
}
