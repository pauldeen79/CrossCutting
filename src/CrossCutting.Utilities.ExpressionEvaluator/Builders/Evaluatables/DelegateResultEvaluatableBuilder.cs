namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class DelegateResultEvaluatableBuilder
{
    public DelegateResultEvaluatableBuilder(Func<Result<object?>> value)
    {
        _value = value;
    }
}

public partial class DelegateResultEvaluatableBuilder<T>
{
    public DelegateResultEvaluatableBuilder(Func<Result<T>> value)
    {
        _value = value;
    }
}