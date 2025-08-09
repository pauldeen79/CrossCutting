namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Expressions;

public partial class PropertyNameExpressionBuilder
{
    public PropertyNameExpressionBuilder(string propertyName)
    {
        ArgumentGuard.IsNotNullOrEmpty(propertyName, nameof(propertyName));

        _propertyName = propertyName;
    }
}
