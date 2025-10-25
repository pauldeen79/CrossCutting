namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

internal sealed class OperatorExpression : ExpressionBase
{
    public Result<IExpression> Left { get; }
    public ExpressionTokenType Operator { get; }
    public Result<IExpression> Right { get; }
    public string SourceExpression { get; }

    private readonly ExpressionEvaluatorContext _context;
    private readonly IEnumerable<IBinaryExpressionComponent> _components;

    public OperatorExpression(
        ExpressionEvaluatorContext context,
        Result<IExpression> left,
        ExpressionTokenType @operator,
        Result<IExpression> right,
        string sourceExpression,
        IEnumerable<IBinaryExpressionComponent> components)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(left, nameof(left));
        ArgumentGuard.IsNotNull(right, nameof(right));
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));
        ArgumentGuard.IsNotNull(components, nameof(components));

        _context = context;
        Left = left;
        Operator = @operator;
        Right = right;
        SourceExpression = sourceExpression;
        _components = components;
    }

    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        var results = await new AsyncResultDictionaryBuilder()
            .Add(Constants.LeftExpression, () => Left.Value is not null
                ? Left.Value.EvaluateAsync(context, token)
                : Task.FromResult(Result.FromExistingResult<object?>(Left)))
            .Add(Constants.RightExpression, () => Right.Value is not null
                ? Right.Value.EvaluateAsync(context, token)
                : Task.FromResult(Result.FromExistingResult<object?>(Right)))
            .BuildAsync()
            .ConfigureAwait(false);

        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<object?>(error);
        }

        return Process(Operator, _context, results);
    }

    public override async Task<ExpressionParseResult> ParseAsync(CancellationToken token)
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
            .WithExpressionComponentType(typeof(OperatorExpression))
            .WithSourceExpression(SourceExpression)
            .AddPartResult(leftResult ?? new ExpressionParseResultBuilder().FillFromResult(Left), Constants.LeftExpression)
            .AddPartResult(rightResult ?? new ExpressionParseResultBuilder().FillFromResult(Right), Constants.RightExpression)
            .SetStatusFromPartResults();

        if (!result.IsSuccessful())
        {
            return result;
        }

        var component = _components.FirstOrDefault(x => x.Supports(Operator));

        return component is not null
            ? result.WithResultType(GetResultType(leftResult, component))
            : result.WithStatus(ResultStatus.Invalid).WithErrorMessage($"Unsupported operator: {Operator}");
    }

    private Result<object?> Process(ExpressionTokenType @operator, ExpressionEvaluatorContext context, IReadOnlyDictionary<string, Result> results)
        => _components
            .Select(x => Result.WrapException(() => x.Process(@operator, context, results)))
            .WhenNotContinue(() => Result.Invalid<object?>($"Unsupported operator: {Operator}"));

    private static Type? GetResultType(ExpressionParseResult? leftResult, IBinaryExpressionComponent component)
        => component.HasBooleanResult
            ? typeof(bool)
            : leftResult?.ResultType;
}
