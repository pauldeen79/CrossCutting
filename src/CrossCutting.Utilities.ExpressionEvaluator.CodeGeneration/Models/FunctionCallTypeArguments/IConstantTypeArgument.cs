namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IConstantTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required] Type Value { get; }
}
