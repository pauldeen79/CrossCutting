namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class NotBetweenConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}