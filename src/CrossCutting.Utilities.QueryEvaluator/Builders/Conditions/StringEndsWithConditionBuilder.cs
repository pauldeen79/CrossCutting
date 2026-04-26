namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class StringEndsWithConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}