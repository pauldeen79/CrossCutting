namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class SmallerThanOrEqualConditionBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTyped();
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}