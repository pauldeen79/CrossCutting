namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface ISourceExpressionContainer
{
    [Required][ValidateObject] IEvaluatable SourceExpression { get; set; }
}

internal interface ISourceExpressionContainer<T>
{
    [Required][ValidateObject] IEvaluatable<T> SourceExpression { get; set; }
}