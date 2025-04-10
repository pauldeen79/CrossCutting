namespace CrossCutting.Utilities.ExpressionEvaluator.OperatorExpressions;

internal sealed class UnaryExpression : IOperatorExpression
{
    public ExpressionTokenType Operator { get; }
    public Result<IOperatorExpression> Operand { get; }

    public UnaryExpression(ExpressionTokenType operatorType, Result<IOperatorExpression> operand)
    {
        Operator = operatorType;
        Operand = operand;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context, Func<string, Result<object?>> @delegate)
    {
        var results = new ResultDictionaryBuilder<object?>()
            .Add(Constants.Expression, () => Operand.Value?.Evaluate(context, @delegate) ?? Result.FromExistingResult<object?>(Operand))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return error;
        }

        return Result.Success<object?>(!GetBooleanValue(results.GetValue(Constants.Expression)));
    }

    private static bool GetBooleanValue(object? value)
    {
        if (value is bool b)
        {
            return b;
        }
        else if (value is string s)
        {
            // design decision: if it's a string, then do a null or empty check
            return !string.IsNullOrEmpty(s);
        }
        else
        {
            // design decision: if it's not a boolean, then do a null check
            return value is not null;
        }
    }
}
