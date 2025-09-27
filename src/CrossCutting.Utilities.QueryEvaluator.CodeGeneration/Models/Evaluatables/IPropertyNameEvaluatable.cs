namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IPropertyNameEvaluatable
{
    [Required, ValidateObject]
    IEvaluatable SourceExpression { get; }

    [Required]
    string PropertyName { get; }
}
