namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class GreaterOrEqualOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}