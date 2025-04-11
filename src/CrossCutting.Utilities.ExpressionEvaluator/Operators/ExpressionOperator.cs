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
        
        var valueResult = context.Parse(Expression);

        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(OperatorExpression))
            .WithSourceExpression(context.Expression)
            .WithResultType(valueResult.ResultType)
            .AddPartResult(valueResult, Constants.Expression)
            .SetStatusFromPartResults();

        return result;
    }
}
