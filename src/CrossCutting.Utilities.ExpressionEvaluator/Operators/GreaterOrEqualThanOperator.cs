namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

public class GreaterOrEqualThanOperator : IOperator, IOperatorBuilder
{
    public string OperatorExpression => ">=";

    public int Order => 20;

    public Result<bool> Evaluate(OperatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return GreaterOrEqualThan.Evaluate(context.LeftExpression, context.RightExpression);
    }

    public IOperatorBuilder ToBuilder() => this;
    public IOperator Build() => this;
}
