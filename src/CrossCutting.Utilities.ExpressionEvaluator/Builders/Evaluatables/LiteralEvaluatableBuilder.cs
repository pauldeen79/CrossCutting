namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class LiteralEvaluatableBuilder
{
    public LiteralEvaluatableBuilder(object? value)
    {
        _value = value;
    }
}

public partial class LiteralEvaluatableBuilder<T>
{
    public LiteralEvaluatableBuilder(T value)
    {
        _value = value;
    }
}