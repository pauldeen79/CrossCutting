namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class PropertyNameEvaluatableBuilder : IEvaluatableBuilder
{
    partial void SetDefaultValues()
    {
        _sourceExpression = new ContextEvaluatableBuilder();
    }

    public PropertyNameEvaluatableBuilder(string propertyName)
    {
        _sourceExpression = new ContextEvaluatableBuilder();
        _propertyName = propertyName;
    }
}
