namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface ICompareExpressionContainer : ISourceExpressionContainer
{
    [Required][ValidateObject] IEvaluatable CompareExpression { get; set; }
}
