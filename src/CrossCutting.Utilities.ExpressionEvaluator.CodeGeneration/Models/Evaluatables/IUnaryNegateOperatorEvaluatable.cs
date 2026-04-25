namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IUnaryNegateOperatorEvaluatable : IEvaluatableBase, IUnaryOperator, IEvaluatable<bool>
{
    //TODO: Try to use this instead of IUnaryOperator
    // [Required, ValidateObject] IEvaluatable<bool> Operand { get; }
}
