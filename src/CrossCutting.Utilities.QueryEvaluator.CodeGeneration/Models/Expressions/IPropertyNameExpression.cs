namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface IPropertyNameExpression
{
    [Required, ValidateObject]
    IExpression SourceExpression { get; }

    [Required]
    string PropertyName { get; }
}
