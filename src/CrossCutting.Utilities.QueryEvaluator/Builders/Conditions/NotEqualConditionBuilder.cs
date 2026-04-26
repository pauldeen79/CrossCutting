namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class NotEqualConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}