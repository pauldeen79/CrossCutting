namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class GreaterThanOrEqualConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}