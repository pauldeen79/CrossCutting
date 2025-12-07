namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Abstractions;

internal interface IBinaryOperator
{
    [Required, ValidateObject] IEvaluatable LeftOperand { get; }
    [Required, ValidateObject] IEvaluatable RightOperand { get; }
}
