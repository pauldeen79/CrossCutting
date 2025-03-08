namespace CrossCutting.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IFunctionTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required][ValidateObject] IFunctionCall Function { get; }
}
