namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class PropertyNameEvaluatableBuilder : IEvaluatableBuilder
{
    partial void SetDefaultValues()
    {
        _sourceExpression = new ContextEvaluatable(); //TODO: Review why we can't use builder here
    }

    public PropertyNameEvaluatableBuilder(string propertyName)
    {
        _sourceExpression = new ContextEvaluatable(); //TODO: Review why we can't use builder here
        _propertyName = propertyName;
    }
}
