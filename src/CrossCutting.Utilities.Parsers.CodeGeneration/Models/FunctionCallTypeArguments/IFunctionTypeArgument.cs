namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IFunctionTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required][ValidateObject] IFunctionCall Function { get; }
}
