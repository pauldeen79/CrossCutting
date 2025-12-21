namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class LiteralResultEvaluatableBuilder
{
    public LiteralResultEvaluatableBuilder(Result<object?> value)
    {
        _value = value;
    }
}

public partial class LiteralResultEvaluatableBuilder<T>
{
    public LiteralResultEvaluatableBuilder(Result<T> value)
    {
        _value = value;
    }
}