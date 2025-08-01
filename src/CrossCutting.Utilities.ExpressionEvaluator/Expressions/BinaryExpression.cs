namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class BinaryExpression : IExpression
{
    public Result<IExpression> Left { get; }
    public ExpressionTokenType Operator { get; }
    public Result<IExpression> Right { get; }
    public string SourceExpression { get; }

    private readonly ExpressionEvaluatorContext _context;

    private readonly Dictionary<ExpressionTokenType, Func<IReadOnlyDictionary<string, Result>, Result<object?>>> _dictionary;

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

        _dictionary = new()
        {
            { ExpressionTokenType.Plus, results => Add.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider) },
            { ExpressionTokenType.Minus, results => Subtract.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider) },
            { ExpressionTokenType.Multiply, results => Multiply.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider) },
            { ExpressionTokenType.Divide, results => Divide.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider) },
            { ExpressionTokenType.Equal, results => Equal.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.StringComparison) },
            { ExpressionTokenType.NotEqual, results => NotEqual.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.StringComparison) },
            { ExpressionTokenType.Less, results => SmallerThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)) },
            { ExpressionTokenType.LessOrEqual, results => SmallerOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)) },
            { ExpressionTokenType.Greater, results => GreaterThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)) },
            { ExpressionTokenType.GreaterOrEqual, results => GreaterOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)) },
            { ExpressionTokenType.And, results => EvaluateAnd(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)) },
            { ExpressionTokenType.Or, results => EvaluateOr(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)) },
            { ExpressionTokenType.Modulo, results => Modulus.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider) },
            { ExpressionTokenType.Exponentiation, results => Power.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), _context.Settings.FormatProvider) },
        };
    }

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        var results = await new AsyncResultDictionaryBuilder()
            .Add(Constants.LeftExpression, Left.Value is not null
                ? Left.Value.EvaluateAsync(context, token)
                : Task.FromResult(Result.FromExistingResult<object?>(Left)))
            .Add(Constants.RightExpression, Right.Value is not null
                ? Right.Value.EvaluateAsync(context, token)
                : Task.FromResult(Result.FromExistingResult<object?>(Right)))
            .Build()
            .ConfigureAwait(false);

        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<object?>(error);
        }

        return Result.WrapException(() => _dictionary.TryGetValue(Operator, out var action)
            ? action(results)
            : Result.Invalid<object?>($"Unsupported operator: {Operator}"));
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

        return _dictionary.ContainsKey(Operator)
            ? result
            : result.WithStatus(ResultStatus.Invalid).WithErrorMessage($"Unsupported operator: {Operator}");
    }

    private static Result<object?> EvaluateAnd(object? left, object? right)
        => Result.Success<object?>(left.IsTruthy() && right.IsTruthy());

    private static Result<object?> EvaluateOr(object? left, object? right)
        => Result.Success<object?>(left.IsTruthy() || right.IsTruthy());
}
