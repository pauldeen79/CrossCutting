namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IConstantResultTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required] Result<Type> Value { get; }
}
