namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class EqualConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}