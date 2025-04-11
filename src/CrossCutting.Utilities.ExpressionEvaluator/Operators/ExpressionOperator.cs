namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

internal sealed class ExpressionOperator : IOperator
{
    private string Expression { get; }

    public ExpressionOperator(string expression)
    {
        Expression = expression;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context) => context.Evaluate(Expression);

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
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
