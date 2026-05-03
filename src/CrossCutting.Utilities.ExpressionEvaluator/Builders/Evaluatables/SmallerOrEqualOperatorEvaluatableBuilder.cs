namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class SmallerOrEqualOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}