namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class NotEqualOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}