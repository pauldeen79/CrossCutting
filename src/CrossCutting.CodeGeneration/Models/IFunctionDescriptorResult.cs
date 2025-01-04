namespace CrossCutting.CodeGeneration.Models;

internal interface IFunctionDescriptorResult
{
    ResultStatus Status { get; }
    [Required(AllowEmptyStrings = true)] string Value { get; }
    [Required(AllowEmptyStrings = true)] string ValueType { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
}
