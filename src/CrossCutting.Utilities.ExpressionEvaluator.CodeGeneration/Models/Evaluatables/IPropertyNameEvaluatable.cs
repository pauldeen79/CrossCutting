namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IPropertyNameEvaluatable : IEvaluatableBase
{
    [Required, ValidateObject]
    IEvaluatable SourceExpression { get; }

    [Required]
    string PropertyName { get; }
}
