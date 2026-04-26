namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class GreaterOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}