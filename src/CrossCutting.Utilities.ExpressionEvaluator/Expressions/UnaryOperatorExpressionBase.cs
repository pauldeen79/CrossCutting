namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public abstract class UnaryOperatorExpressionBase : IExpression
{
    public Result<IExpression> Operand { get; }
    public string SourceExpression { get; }

    protected UnaryOperatorExpressionBase(
        Result<IExpression> operand,
        string sourceExpression)
    {
        ArgumentGuard.IsNotNull(operand, nameof(operand));
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));

        Operand = operand;
        SourceExpression = sourceExpression;
    }

    public abstract Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);

    public async Task<ExpressionParseResult> ParseAsync(CancellationToken token)
    {
        ExpressionParseResult? operandResult = null;

        if (Operand.IsSuccessful() && Operand.Value is not null)
        {
            operandResult = await Operand.Value.ParseAsync(token).ConfigureAwait(false);
        }

        var result = new ExpressionParseResultBuilder()
            .WithExpressionComponentType(GetType())
            .WithSourceExpression(SourceExpression)
            .AddPartResult(operandResult ?? new ExpressionParseResultBuilder().FillFromResult(Operand), Constants.Expression)
            .SetStatusFromPartResults();

        if (!result.IsSuccessful())
        {
            return result;
        }

        return result.WithResultType(GetResultType(operandResult));
    }

    protected Task<IReadOnlyDictionary<string, Result>> EvaluateAsResultDictionaryAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new AsyncResultDictionaryBuilder()
            .Add(Constants.Expression, () => Operand.Value is not null
                ? Operand.Value.EvaluateAsync(context, token)
                : Task.FromResult(Result.FromExistingResult<object?>(Operand)))
            .BuildAsync(token);

    public abstract IEvaluatableBuilder ToBuilder();

    protected abstract Type? GetResultType(ExpressionParseResult? leftResult);
}
