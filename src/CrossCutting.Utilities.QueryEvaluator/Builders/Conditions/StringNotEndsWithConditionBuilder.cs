namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class StringNotEndsWithConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}