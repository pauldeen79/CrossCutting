namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public abstract class BinaryOperatorExpressionBase : IExpression
{
    public Result<IExpression> Left { get; }
    public Result<IExpression> Right { get; }
    public string SourceExpression { get; }

    protected BinaryOperatorExpressionBase(
        Result<IExpression> left,
        Result<IExpression> right,
        string sourceExpression)
    {
        ArgumentGuard.IsNotNull(left, nameof(left));
        ArgumentGuard.IsNotNull(right, nameof(right));
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));

        Left = left;
        Right = right;
        SourceExpression = sourceExpression;
    }

    public abstract Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);

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
            .WithExpressionComponentType(GetType())
            .WithSourceExpression(SourceExpression)
            .AddPartResult(leftResult ?? new ExpressionParseResultBuilder().FillFromResult(Left), Constants.LeftExpression)
            .AddPartResult(rightResult ?? new ExpressionParseResultBuilder().FillFromResult(Right), Constants.RightExpression)
            .SetStatusFromPartResults();

        if (!result.IsSuccessful())
        {
            return result;
        }

        return result.WithResultType(GetResultType(leftResult));
    }

    protected Task<IReadOnlyDictionary<string, Result>> EvaluateAsResultDictionaryAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new AsyncResultDictionaryBuilder()
            .Add(Constants.LeftExpression, () => Left.Value is not null
                ? Left.Value.EvaluateAsync(context, token)
                : Task.FromResult(Result.FromExistingResult<object?>(Left)))
            .Add(Constants.RightExpression, () => Right.Value is not null
                ? Right.Value.EvaluateAsync(context, token)
                : Task.FromResult(Result.FromExistingResult<object?>(Right)))
            .BuildAsync(token);

    public abstract IEvaluatableBuilder ToBuilder();

    protected abstract Type? GetResultType(ExpressionParseResult? leftResult);
}
