namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IPropertyNameEvaluatable : IEvaluatableBase, IUnaryOperator
{
    [Required]
    string PropertyName { get; }
}
