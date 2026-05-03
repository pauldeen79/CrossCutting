namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class InConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}