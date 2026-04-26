namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;

public partial class BinaryAndOperatorEvaluatableBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
}