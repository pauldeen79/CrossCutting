namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class StringNotEqualsConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}