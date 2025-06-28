namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models;

internal interface IFunctionDescriptorResult
{
    ResultStatus Status { get; }
    [Required(AllowEmptyStrings = true)] string Value { get; }
    Type? ValueType { get; }
    [Required(AllowEmptyStrings = true)] string Description { get; }
}
