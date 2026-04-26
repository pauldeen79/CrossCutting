namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class GreaterThanConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}