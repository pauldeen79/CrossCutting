namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class BetweenConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}