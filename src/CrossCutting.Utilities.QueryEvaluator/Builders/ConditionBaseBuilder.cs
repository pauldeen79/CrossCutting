namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders;

public partial class ConditionBaseBuilder : IEvaluatableBuilder
{
    IEvaluatable IBuilder<IEvaluatable>.Build() => Build();
}
