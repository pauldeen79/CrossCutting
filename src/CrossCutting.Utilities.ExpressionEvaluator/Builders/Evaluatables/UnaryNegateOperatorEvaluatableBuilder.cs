namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class UnaryNegateOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}