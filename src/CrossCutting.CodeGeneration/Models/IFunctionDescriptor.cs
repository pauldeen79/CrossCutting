namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionDescriptor
{
    [Required] string FunctionName { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionDescriptorArgumentBase> Arguments { get; }
}
