namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IExpressionParseResult
{
    [Required(AllowEmptyStrings = true)] string SourceExpression { get; }
    ResultStatus Status { get; }
    [Required] IReadOnlyCollection<ValidationError> ValidationErrors { get; }
    string? ErrorMessage { get; }
    Type ExpressionType { get; }
    Type? ResultType { get; }
    [Required][ValidateObject] IReadOnlyCollection<IExpressionParsePartResult> PartResults { get; }
}
