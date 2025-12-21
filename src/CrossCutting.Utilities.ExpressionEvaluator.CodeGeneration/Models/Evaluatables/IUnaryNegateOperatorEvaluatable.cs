namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IUnaryNegateOperatorEvaluatable : IEvaluatableBase, IEvaluatable<bool>
{
    [Required, ValidateObject] IEvaluatable Operand { get; }
}
