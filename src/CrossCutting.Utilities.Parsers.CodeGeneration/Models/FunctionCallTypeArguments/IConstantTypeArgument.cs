namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IConstantTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required] Type Value { get; }
}
