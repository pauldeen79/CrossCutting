namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class SmallerThanOrEqualConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}