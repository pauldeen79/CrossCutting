namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IExpressionParsePartResult : IParseResult
{
    [Required] string PartName { get; }
    [Required][ValidateObject] IReadOnlyCollection<IExpressionParsePartResult> PartResults { get; }
}
