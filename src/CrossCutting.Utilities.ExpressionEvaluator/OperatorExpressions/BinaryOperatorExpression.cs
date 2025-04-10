namespace CrossCutting.Utilities.ExpressionEvaluator.OperatorExpressions;

internal sealed class BinaryOperatorExpression : IOperatorExpression
{
    public Result<IOperatorExpression> Left { get; }
    public ExpressionTokenType Operator { get; }
    public Result<IOperatorExpression> Right { get; }

    public BinaryOperatorExpression(Result<IOperatorExpression> left, ExpressionTokenType op, Result<IOperatorExpression> right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context, Func<string, Result<object?>> @delegate)
    {
        var results = new ResultDictionaryBuilder<object?>()
            .Add(Constants.LeftExpression, () => Left.Value?.Evaluate(context, @delegate) ?? Result.FromExistingResult<object?>(Left))
            .Add(Constants.RightExpression, () => Right.Value?.Evaluate(context, @delegate) ?? Result.FromExistingResult<object?>(Right))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return error;
        }

        return Operator switch
        {
            ExpressionTokenType.Plus => Add.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            ExpressionTokenType.Minus => Subtract.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            ExpressionTokenType.Multiply => Multiply.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            ExpressionTokenType.Divide => Divide.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            ExpressionTokenType.EqualEqual => Equal.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.StringComparison).TryCastAllowNull<object?>(),
            ExpressionTokenType.NotEqual => NotEqual.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.StringComparison).TryCastAllowNull<object?>(),
            ExpressionTokenType.Less => SmallerThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            ExpressionTokenType.LessEqual => SmallerOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            ExpressionTokenType.Greater => GreaterThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            ExpressionTokenType.GreaterEqual => GreaterOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            ExpressionTokenType.AndAnd => EvaluateAnd(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            ExpressionTokenType.OrOr => EvaluateOr(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            _ => Result.Invalid<object?>("Unsupported operator")
        };
    }

    private static Result<object?> EvaluateAnd(object? left, object? right)
        => Result.Success<object?>(GetBooleanValue(left) && GetBooleanValue(right));

    private static Result<object?> EvaluateOr(object? left, object? right)
        => Result.Success<object?>(GetBooleanValue(left) || GetBooleanValue(right));

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
