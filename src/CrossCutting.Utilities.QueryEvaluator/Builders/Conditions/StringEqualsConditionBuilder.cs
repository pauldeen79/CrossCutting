namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class StringEqualsConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}