namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.FunctionCallArguments;

internal interface IExpressionArgument : IFunctionCallArgumentBase
{
    [Required] string Expression { get; }
}
