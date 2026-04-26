namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class StringStartsWithConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}