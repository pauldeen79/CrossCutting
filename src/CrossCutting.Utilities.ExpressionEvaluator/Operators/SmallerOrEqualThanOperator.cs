namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

public class SmallerOrEqualThanOperator : IOperator, IOperatorBuilder
{
    public string OperatorExpression => "<=";

    public int Order => 10;

    public Result<bool> Evaluate(OperatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return SmallerOrEqualThan.Evaluate(context.LeftExpression, context.RightExpression);
    }

    public IOperatorBuilder ToBuilder() => this;
    public IOperator Build() => this;
}
