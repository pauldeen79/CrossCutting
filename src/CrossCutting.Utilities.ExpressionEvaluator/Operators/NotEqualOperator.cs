namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

public class NotEqualOperator : IOperator
{
    public string OperatorExpression => "!=";

    public int Order => 60;

    public Result<bool> Evaluate(OperatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return NotEqual.Evaluate(context.LeftExpression, context.RightExpression, context.StringComparison);
    }
}
