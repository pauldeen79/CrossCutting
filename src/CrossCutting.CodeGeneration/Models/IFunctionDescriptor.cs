namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionDescriptor
{
    [Required] string Name { get; }
    [Description("Optional Id to use in case of function overload resolution")] [Required(AllowEmptyStrings = true)] string Id { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionDescriptorArgument> Arguments { get; }
}
