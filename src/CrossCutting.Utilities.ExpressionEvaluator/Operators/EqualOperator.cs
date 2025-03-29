namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

public class EqualOperator : IOperator
{
    public string OperatorExpression => "==";

    public int Order => 50;

    public Result<bool> Evaluate(OperatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return Equal.Evaluate(context.LeftExpression, context.RightExpression, context.StringComparison);
    }
}
