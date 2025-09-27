namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface IPropertyNameExpression
{
    [Required, ValidateObject]
    public IExpression SourceExpression { get; }

    [Required]
    public string PropertyName { get; }
}
