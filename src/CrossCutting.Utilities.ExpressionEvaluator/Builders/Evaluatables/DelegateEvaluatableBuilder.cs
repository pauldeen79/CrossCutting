namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class DelegateEvaluatableBuilder
{
    public DelegateEvaluatableBuilder(Func<object?> value)
    {
        _value = value;
    }
}
