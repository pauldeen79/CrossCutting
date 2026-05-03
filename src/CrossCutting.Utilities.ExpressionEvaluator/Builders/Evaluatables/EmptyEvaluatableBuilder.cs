namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class EmptyEvaluatableBuilder<T> : IEvaluatableBuilder<T>
{
    IEvaluatable<T> IEvaluatableBuilder<T>.BuildTyped() => BuildTyped();
}