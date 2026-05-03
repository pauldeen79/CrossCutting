namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class NotInConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}