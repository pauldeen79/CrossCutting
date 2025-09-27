namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Expressions;

public sealed class PropertyNameExpressionBuilder : IExpressionBuilder
{
    [Required, ValidateObject]
    public IExpressionBuilder SourceExpression { get; set; }

    [Required]
    public string PropertyName { get; set; }

    public PropertyNameExpressionBuilder()
    {
        SourceExpression = new ContextExpressionBuilder();
        PropertyName = string.Empty;
    }

    public PropertyNameExpressionBuilder(PropertyNameExpression propertyNameExpression)
    {
        propertyNameExpression = ArgumentGuard.IsNotNull(propertyNameExpression, nameof(propertyNameExpression));

        SourceExpression = propertyNameExpression.SourceExpression.ToBuilder();
        PropertyName = propertyNameExpression.PropertyName;
    }

    public PropertyNameExpressionBuilder WithPropertyName(string propertyName)
        => this.With(x => x.PropertyName = propertyName);

    public PropertyNameExpressionBuilder WithSourceExpression(IExpressionBuilder sourceExpression)
        => this.With(x => x.SourceExpression = sourceExpression);

    public IExpression Build()
        => new PropertyNameExpression(SourceExpression?.Build()!, PropertyName);
}
