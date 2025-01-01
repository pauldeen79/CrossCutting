namespace CrossCutting.CodeGeneration.Models.Abstractions;

public interface IFunctionDescriptorArgument
{
    [Required] string Name { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
    bool IsRequired { get; }
}
