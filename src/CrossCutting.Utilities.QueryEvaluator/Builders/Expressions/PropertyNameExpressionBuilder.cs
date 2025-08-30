namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Expressions;

public partial class PropertyNameExpressionBuilder
{
    public PropertyNameExpressionBuilder(string propertyName) : this(new ContextExpressionBuilder(), propertyName)
    {
    }

    public PropertyNameExpressionBuilder(IExpressionBuilder expression, string propertyName)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));
        ArgumentGuard.IsNotNullOrEmpty(propertyName, nameof(propertyName));

        _expression = expression;
        _propertyName = propertyName;
    }

    partial void SetDefaultValues()
    {
        _expression = new ContextExpressionBuilder();
    }
}
