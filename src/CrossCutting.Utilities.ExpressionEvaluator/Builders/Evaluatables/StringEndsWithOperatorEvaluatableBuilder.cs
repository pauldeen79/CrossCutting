namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class StringEndsWithOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}