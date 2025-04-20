namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Abstractions;

internal interface IParseResult
{
    ResultStatus Status { get; }
    [Required] IReadOnlyCollection<ValidationError> ValidationErrors { get; }
    string? ErrorMessage { get; }

    [Required(AllowEmptyStrings = true)] string SourceExpression { get; }
    Type? ExpressionComponentType { get; }
    Type? ResultType { get; }
}
