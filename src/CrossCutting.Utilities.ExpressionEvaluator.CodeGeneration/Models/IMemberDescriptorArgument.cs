namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IMemberDescriptorArgument
{
    [Required] string Name { get; }
    [Required] Type Type { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
    bool IsRequired { get; }
}
