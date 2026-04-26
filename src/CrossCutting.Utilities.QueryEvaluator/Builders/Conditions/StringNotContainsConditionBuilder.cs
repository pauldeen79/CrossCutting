namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class StringNotContainsConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}