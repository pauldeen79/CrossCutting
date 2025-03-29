namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

public class GreaterThanOperator : IOperator
{
    public string OperatorExpression => ">";

    public int Order => 40;

    public Result<bool> Evaluate(OperatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return GreaterThan.Evaluate(context.LeftExpression, context.RightExpression);
    }
}
