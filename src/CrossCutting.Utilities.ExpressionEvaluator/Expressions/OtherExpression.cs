namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class OtherExpression : IExpression
{
    private string Expression { get; }

    public OtherExpression(string expression)
    {
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        Expression = expression;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Evaluate(Expression);
    }

    public Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.EvaluateTyped<T>(Expression);
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        
        return context.Parse(Expression);
    }
}
