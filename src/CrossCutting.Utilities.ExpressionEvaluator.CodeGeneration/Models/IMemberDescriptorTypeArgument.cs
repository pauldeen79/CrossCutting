namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IMemberDescriptorTypeArgument
{
    [Required] string Name { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
}
