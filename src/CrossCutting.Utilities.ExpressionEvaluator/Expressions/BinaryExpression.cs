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

    public async Task<Result<object?>> EvaluateAsync(CancellationToken token)
    {
        var results = await new AsyncResultDictionaryBuilder()
            .Add(nameof(Constants.LeftExpression), Left.Value is not null ? Left.Value.EvaluateAsync(token) : Task.FromResult(Result.FromExistingResult<object?>(Left)))
            .Add(nameof(Constants.RightExpression), Right.Value is not null ? Right.Value.EvaluateAsync(token) : Task.FromResult(Result.FromExistingResult<object?>(Right)))
            .Build()
            .ConfigureAwait(false);

        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<object?>(error);
        }

        return Result.WrapException<object?>(() => Operator switch
        {
            ExpressionTokenType.Plus => Add.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
            ExpressionTokenType.Minus => Subtract.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
            ExpressionTokenType.Multiply => Multiply.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
            ExpressionTokenType.Divide => Divide.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
            ExpressionTokenType.Equal => Equal.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.StringComparison),
            ExpressionTokenType.NotEqual => NotEqual.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.StringComparison),
            ExpressionTokenType.Less => SmallerThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            ExpressionTokenType.LessOrEqual => SmallerOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            ExpressionTokenType.Greater => GreaterThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            ExpressionTokenType.GreaterOrEqual => GreaterOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            ExpressionTokenType.And => EvaluateAnd(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            ExpressionTokenType.Or => EvaluateOr(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            ExpressionTokenType.Modulo => Modulus.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
            ExpressionTokenType.Exponentiation => Power.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider),
            _ => Result.Invalid<object?>($"Unsupported operator: {Operator}")
        });
    }

    public async Task<ExpressionParseResult> ParseAsync(CancellationToken token)
    {
        ExpressionParseResult? leftResult = null;
        ExpressionParseResult? rightResult = null;

        if (Left.IsSuccessful() && Left.Value is not null)
        {
            leftResult = await Left.Value.ParseAsync(token).ConfigureAwait(false);
        }

        if (Right.IsSuccessful() && Right.Value is not null)
        {
            rightResult = await Right.Value.ParseAsync(token).ConfigureAwait(false);
        }

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
