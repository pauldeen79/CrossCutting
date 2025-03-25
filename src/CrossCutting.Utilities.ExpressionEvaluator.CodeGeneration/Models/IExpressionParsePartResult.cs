namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IExpressionParsePartResult
{
    [Required] string PartName { get; }
    //TODO: Add interesting properties from Result (ValidationErrors, ErrorMessage, ResultStatus?), and remove Result property
    [Required] Result Result { get; }
    string? SourceExpression { get; }
    Type? ExpressionType { get; }
    Type? ResultType { get; }
    [Required][ValidateObject] IReadOnlyCollection<IExpressionParsePartResult> PartResults { get; }
}
