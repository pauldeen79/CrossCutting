namespace CrossCutting.Utilities.ExpressionEvaluator.OperatorExpressions;

internal sealed class NonBinaryOperatorExpression : IOperatorExpression
{
    private string Value { get; }

    public NonBinaryOperatorExpression(string value)
    {
        Value = value;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context, Func<string, Result<object?>> @delegate) => @delegate(Value);
}
