namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IFunctionTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required][ValidateObject] IFunctionCall Function { get; }
}
