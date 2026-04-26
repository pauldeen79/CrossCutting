namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class NullOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}