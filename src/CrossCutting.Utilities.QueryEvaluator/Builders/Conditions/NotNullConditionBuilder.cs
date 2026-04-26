namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class NotNullConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}