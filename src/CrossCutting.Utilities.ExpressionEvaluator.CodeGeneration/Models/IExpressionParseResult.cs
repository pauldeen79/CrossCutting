namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IExpressionParseResult
{
    [Required(AllowEmptyStrings = true)] string SourceExpression { get; }
    Type ExpressionType { get; }
    Type? ResultType { get; }
    [Required][ValidateObject] IReadOnlyCollection<IExpressionParsePartResult> PartResults { get; }
}
