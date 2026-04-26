namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class SmallerThanConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}