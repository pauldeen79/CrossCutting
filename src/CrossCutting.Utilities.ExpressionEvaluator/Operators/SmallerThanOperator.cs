namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

public class SmallerThanOperator : IOperator, IOperatorBuilder
{
    public string OperatorExpression => "<";

    public int Order => 30;

    public Result<bool> Evaluate(OperatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return SmallerThan.Evaluate(context.LeftExpression, context.RightExpression);
    }

    public IOperatorBuilder ToBuilder() => this;
    public IOperator Build() => this;
}
