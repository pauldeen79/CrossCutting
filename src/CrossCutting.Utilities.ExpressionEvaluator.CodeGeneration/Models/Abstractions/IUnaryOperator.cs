namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Abstractions;

internal interface IUnaryOperator
{
    [Required, ValidateObject] IEvaluatable Operand { get; }
}