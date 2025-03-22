namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IFunctionDescriptorTypeArgument
{
    [Required] string Name { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
}
