namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Abstractions;

internal interface IBinaryStringOperator
{
    [Required, ValidateObject] IEvaluatable<string> LeftOperand { get; }
    [Required, ValidateObject] IEvaluatable<string> RightOperand { get; }

    StringComparison StringComparison { get; }
}
