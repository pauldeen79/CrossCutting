namespace CrossCutting.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IConstantTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required] Type Value { get; }
}
