namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IConstantResultTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required] Result<Type> Value { get; }
}
