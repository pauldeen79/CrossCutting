namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface ISingleExpressionContainer
{
    [Required][ValidateObject] IExpression FirstExpression { get; set; }
}
