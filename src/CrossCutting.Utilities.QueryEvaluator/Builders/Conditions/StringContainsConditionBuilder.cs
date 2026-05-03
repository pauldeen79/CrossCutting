namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class StringContainsConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}