namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class StringStartsWithOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}