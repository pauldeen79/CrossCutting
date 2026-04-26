namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class LiteralEvaluatableBuilder
{
    public LiteralEvaluatableBuilder(object? value) => _value = value;
}

public partial class LiteralEvaluatableBuilder<T> : IEvaluatableBuilder<T>
{
    public LiteralEvaluatableBuilder(T value) => _value = value;

    IEvaluatable<T> IEvaluatableBuilder<T>.BuildTyped() => BuildTyped();
}