namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class BinaryExpression : IExpression
{
    public Result<IExpression> Left { get; }
    public ExpressionTokenType Operator { get; }
    public Result<IExpression> Right { get; }
    public string SourceExpression { get; }

    private readonly ExpressionEvaluatorContext _context;

    public BinaryExpression(
        ExpressionEvaluatorContext context,
        Result<IExpression> left,
        ExpressionTokenType @operator,
        Result<IExpression> right,
        string sourceExpression)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(left, nameof(left));
        ArgumentGuard.IsNotNull(right, nameof(right));
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));

        _context = context;
        Left = left;
        Operator = @operator;
        Right = right;
        SourceExpression = sourceExpression;
    }

    public Result<object?> Evaluate()
    {
        return new ResultDictionaryBuilder()
            .Add(Constants.LeftExpression, () => Left.Value?.Evaluate() ?? Result.FromExistingResult<object?>(Left))
            .Add(Constants.RightExpression, () => Right.Value?.Evaluate() ?? Result.FromExistingResult<object?>(Right))
            .Build()
            .OnSuccess(results =>
                Operator switch
                {
                    ExpressionTokenType.Plus => Add.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
                    ExpressionTokenType.Minus => Subtract.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
                    ExpressionTokenType.Multiply => Multiply.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
                    ExpressionTokenType.Divide => Divide.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
                    ExpressionTokenType.Equal => Equal.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.StringComparison).TryCastAllowNull<object?>(),
                    ExpressionTokenType.NotEqual => NotEqual.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.StringComparison).TryCastAllowNull<object?>(),
                    ExpressionTokenType.Less => SmallerThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
                    ExpressionTokenType.LessOrEqual => SmallerOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
                    ExpressionTokenType.Greater => GreaterThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
                    ExpressionTokenType.GreaterOrEqual => GreaterOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
                    ExpressionTokenType.And => EvaluateAnd(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
                    ExpressionTokenType.Or => EvaluateOr(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
                    ExpressionTokenType.Modulo => Modulus.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
                    ExpressionTokenType.Exponentiation => Power.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
                    _ => Result.Invalid<object?>($"Unsupported operator: {Operator}")
                });
    }

    public ExpressionParseResult Parse()
    {
        var leftResult = Left.Value?.Parse();
        var rightResult = Right.Value?.Parse();

        var result = new ExpressionParseResultBuilder()
            .WithExpressionComponentType(typeof(BinaryExpression))
            .WithSourceExpression(SourceExpression)
            .WithResultType(Operator.In(ExpressionTokenType.And, ExpressionTokenType.Or, ExpressionTokenType.Equal, ExpressionTokenType.NotEqual, ExpressionTokenType.Less, ExpressionTokenType.LessOrEqual, ExpressionTokenType.Greater, ExpressionTokenType.GreaterOrEqual)
                ? typeof(bool)
                : leftResult?.ResultType)
            .AddPartResult(leftResult ?? new ExpressionParseResultBuilder().FillFromResult(Left), Constants.LeftExpression)
            .AddPartResult(rightResult ?? new ExpressionParseResultBuilder().FillFromResult(Right), Constants.RightExpression)
            .SetStatusFromPartResults();

        if (!result.IsSuccessful())
        {
            return result;
        }

        return Operator switch
        {
            ExpressionTokenType.Plus or
            ExpressionTokenType.Minus or
            ExpressionTokenType.Multiply or
            ExpressionTokenType.Divide or
            ExpressionTokenType.Equal or
            ExpressionTokenType.NotEqual or
            ExpressionTokenType.Less or
            ExpressionTokenType.LessOrEqual or
            ExpressionTokenType.Greater or
            ExpressionTokenType.GreaterOrEqual or
            ExpressionTokenType.And or
            ExpressionTokenType.Or or
            ExpressionTokenType.Modulo or
            ExpressionTokenType.Exponentiation => result,
            _ => result.WithStatus(ResultStatus.Invalid).WithErrorMessage($"Unsupported operator: {Operator}")
        };
    }

    private static Result<object?> EvaluateAnd(object? left, object? right)
        => Result.Success<object?>(left.IsTruthy() && right.IsTruthy());

    private static Result<object?> EvaluateOr(object? left, object? right)
        => Result.Success<object?>(left.IsTruthy() || right.IsTruthy());
}
