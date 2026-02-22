namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class PropertyNameEvaluatableBuilder : IEvaluatableBuilder
{
    partial void SetDefaultValues()
    {
        _operand = new ContextEvaluatableBuilder();
    }

    public PropertyNameEvaluatableBuilder(string propertyName)
    {
        _operand = new ContextEvaluatableBuilder();
        _propertyName = propertyName;
    }
}
