namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class SmallerOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}