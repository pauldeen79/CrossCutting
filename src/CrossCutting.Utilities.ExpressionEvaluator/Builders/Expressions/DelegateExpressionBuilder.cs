namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Expressions;

public partial class DelegateExpressionBuilder
{
    public DelegateExpressionBuilder(Func<object?> value)
    {
        ArgumentGuard.IsNotNull(value, nameof(value));

        _value = value;
    }
}
