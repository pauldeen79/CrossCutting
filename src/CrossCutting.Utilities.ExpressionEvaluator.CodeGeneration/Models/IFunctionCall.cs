namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IFunctionCall
{
    [Required] string Name { get; }
    [Required][ValidateObject] IReadOnlyCollection<Abstractions.IFunctionCallArgument> Arguments { get; }
    [Required] IReadOnlyCollection<Abstractions.IFunctionCallTypeArgument> TypeArguments { get; }
}
