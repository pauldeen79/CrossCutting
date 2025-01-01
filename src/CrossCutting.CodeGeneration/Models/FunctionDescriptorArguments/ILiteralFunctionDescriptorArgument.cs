namespace CrossCutting.CodeGeneration.Models.FunctionDescriptorArguments;

public interface ILiteralFunctionDescriptorArgument : IFunctionDescriptorArgumentBase
{
    [Required] Type Type { get; }
}
