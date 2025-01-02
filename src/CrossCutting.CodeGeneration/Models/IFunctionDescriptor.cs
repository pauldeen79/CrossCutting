namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionDescriptor
{
    [Required] string FunctionName { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionDescriptorArgument> Arguments { get; }
}
