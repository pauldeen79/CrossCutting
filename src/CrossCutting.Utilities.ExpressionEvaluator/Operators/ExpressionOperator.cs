namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

public sealed class ExpressionOperator : IOperator
{
    private string Expression { get; }

    public ExpressionOperator(string expression)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        Expression = expression;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Evaluate(Expression);
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        
        return context.Parse(Expression);
    }
}
