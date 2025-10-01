namespace CrossCutting.Utilities.ExpressionEvaluator.Builders;

public partial class EvaluatableBaseBuilder
{
    IEvaluatable IBuilder<IEvaluatable>.Build() => Build();
}
