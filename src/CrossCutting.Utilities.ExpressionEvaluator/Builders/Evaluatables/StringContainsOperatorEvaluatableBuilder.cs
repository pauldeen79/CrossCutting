namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class StringContainsOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}