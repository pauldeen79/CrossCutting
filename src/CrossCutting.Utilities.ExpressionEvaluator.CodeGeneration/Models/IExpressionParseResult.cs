namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IExpressionParseResult
{
    //TODO: Add interesting properties from Result (ValidationErrors, ErrorMessage, ResultStatus?)
    [Required(AllowEmptyStrings = true)] string SourceExpression { get; }
    Type ExpressionType { get; }
    Type? ResultType { get; }
    [Required][ValidateObject] IReadOnlyCollection<IExpressionParsePartResult> PartResults { get; }
}
