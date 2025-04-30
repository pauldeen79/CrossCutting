namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IMemberDescriptorResult
{
    ResultStatus Status { get; }
    [Required(AllowEmptyStrings = true)] string Value { get; }
    Type? ValueType { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
}
