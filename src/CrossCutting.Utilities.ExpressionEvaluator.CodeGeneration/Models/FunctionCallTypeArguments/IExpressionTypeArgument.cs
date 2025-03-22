namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IExpressionTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required] string Expression { get; }
}
