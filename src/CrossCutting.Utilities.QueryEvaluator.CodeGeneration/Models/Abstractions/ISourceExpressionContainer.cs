namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface ISourceExpressionContainer
{
    [Required][ValidateObject] IEvaluatable SourceExpression { get; set; }
}
