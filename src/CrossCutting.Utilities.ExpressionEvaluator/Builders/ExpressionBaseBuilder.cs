namespace CrossCutting.Utilities.ExpressionEvaluator.Builders;

public partial class ExpressionBaseBuilder
{
    IExpression IBuilder<IExpression>.Build() => Build();
}
