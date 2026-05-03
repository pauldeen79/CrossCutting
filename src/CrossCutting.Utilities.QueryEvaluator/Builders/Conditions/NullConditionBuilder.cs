namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class NullConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}