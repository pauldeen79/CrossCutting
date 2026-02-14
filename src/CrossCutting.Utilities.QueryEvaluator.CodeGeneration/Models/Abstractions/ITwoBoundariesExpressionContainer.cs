namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface ITwoBoundariesExpressionContainer
{
    [Required][ValidateObject] IEvaluatable LowerBoundExpression { get; set; }
    [Required][ValidateObject] IEvaluatable UpperBoundExpression { get; set; }
}