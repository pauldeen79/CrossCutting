namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;

public partial class StringNotStartsWithConditionBuilder : IEvaluatableBuilder<bool>
{
    protected override IEvaluatable<bool> BuildTypedCore() => BuildTyped();
}