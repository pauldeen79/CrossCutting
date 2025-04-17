namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

public sealed class BinaryOperator : IOperator
{
    public Result<IOperator> Left { get; }
    public OperatorExpressionTokenType Operator { get; }
    public Result<IOperator> Right { get; }
    public string SourceExpression { get; }

    public BinaryOperator(Result<IOperator> left, OperatorExpressionTokenType @operator, Result<IOperator> right, string sourceExpression)
    {
        ArgumentGuard.IsNotNull(left, nameof(left));
        ArgumentGuard.IsNotNull(right, nameof(right));
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));

        Left = left;
        Operator = @operator;
        Right = right;
        SourceExpression = sourceExpression;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var results = new ResultDictionaryBuilder<object?>()
            .Add(Constants.LeftExpression, () => Left.Value?.Evaluate(context) ?? Result.FromExistingResult<object?>(Left))
            .Add(Constants.RightExpression, () => Right.Value?.Evaluate(context) ?? Result.FromExistingResult<object?>(Right))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return error;
        }

        return Operator switch
        {
            OperatorExpressionTokenType.Plus => Add.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            OperatorExpressionTokenType.Minus => Subtract.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            OperatorExpressionTokenType.Multiply => Multiply.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            OperatorExpressionTokenType.Divide => Divide.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            OperatorExpressionTokenType.Equal => Equal.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.StringComparison).TryCastAllowNull<object?>(),
            OperatorExpressionTokenType.NotEqual => NotEqual.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.StringComparison).TryCastAllowNull<object?>(),
            OperatorExpressionTokenType.Less => SmallerThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            OperatorExpressionTokenType.LessOrEqual => SmallerOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            OperatorExpressionTokenType.Greater => GreaterThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            OperatorExpressionTokenType.GreaterOrEqual => GreaterOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            OperatorExpressionTokenType.And => EvaluateAnd(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            OperatorExpressionTokenType.Or => EvaluateOr(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            OperatorExpressionTokenType.Modulo => Modulus.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            OperatorExpressionTokenType.Exponentiation => Power.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            _ => Result.Invalid<object?>($"Unsupported operator: {Operator}")
        };
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var leftResult = Left.Value?.Parse(context);
        var rightResult = Right.Value?.Parse(context);

        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(BinaryOperator))
            .WithSourceExpression(SourceExpression)
            .WithResultType(Operator.In(OperatorExpressionTokenType.And, OperatorExpressionTokenType.Or, OperatorExpressionTokenType.Equal, OperatorExpressionTokenType.NotEqual, OperatorExpressionTokenType.Less, OperatorExpressionTokenType.LessOrEqual, OperatorExpressionTokenType.Greater, OperatorExpressionTokenType.GreaterOrEqual)
                ? typeof(bool)
                : leftResult?.ResultType)
            .AddPartResult(leftResult ?? new ExpressionParseResultBuilder().FillFromResult(Left), Constants.LeftExpression)
            .AddPartResult(rightResult ?? new ExpressionParseResultBuilder().FillFromResult(Right), Constants.RightExpression)
            .SetStatusFromPartResults();

        if (!result.Status.IsSuccessful())
        {
            return result;
        }

        return Operator switch
        {
            OperatorExpressionTokenType.Plus or
            OperatorExpressionTokenType.Minus or
            OperatorExpressionTokenType.Multiply or
            OperatorExpressionTokenType.Divide or
            OperatorExpressionTokenType.Equal or
            OperatorExpressionTokenType.NotEqual or
            OperatorExpressionTokenType.Less or
            OperatorExpressionTokenType.LessOrEqual or
            OperatorExpressionTokenType.Greater or
            OperatorExpressionTokenType.GreaterOrEqual or
            OperatorExpressionTokenType.And or
            OperatorExpressionTokenType.Or or
            OperatorExpressionTokenType.Modulo or
            OperatorExpressionTokenType.Exponentiation => result,
            _ => result.WithStatus(ResultStatus.Invalid).WithErrorMessage($"Unsupported operator: {Operator}")
        };
    }

    private static Result<object?> EvaluateAnd(object? left, object? right)
        => Result.Success<object?>(left.IsTruthy() && right.IsTruthy());

    private static Result<object?> EvaluateOr(object? left, object? right)
        => Result.Success<object?>(left.IsTruthy() || right.IsTruthy());
}
