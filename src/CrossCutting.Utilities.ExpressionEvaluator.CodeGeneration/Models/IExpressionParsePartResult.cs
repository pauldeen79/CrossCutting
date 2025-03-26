namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IExpressionParsePartResult
{
    [Required] string PartName { get; }
    ResultStatus Status { get; }
    [Required] IReadOnlyCollection<ValidationError> ValidationErrors { get; }
    string? ErrorMessage { get; }
    string? SourceExpression { get; }
    Type? ExpressionType { get; }
    Type? ResultType { get; }
    [Required][ValidateObject] IReadOnlyCollection<IExpressionParsePartResult> PartResults { get; }
}
