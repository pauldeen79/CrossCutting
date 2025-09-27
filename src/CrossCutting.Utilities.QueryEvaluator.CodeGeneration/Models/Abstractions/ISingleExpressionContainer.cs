namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface ISingleExpressionContainer
{
    [Required][ValidateObject] IEvaluatable SourceExpression { get; set; }
}
