namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IDoubleExpressionContainer : ISingleExpressionContainer
{
    [Required][ValidateObject] IEvaluatable CompareExpression { get; set; }
}
