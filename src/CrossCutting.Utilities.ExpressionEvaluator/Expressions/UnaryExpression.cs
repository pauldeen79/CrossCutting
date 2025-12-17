namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class UnaryExpression : IExpression<bool>
{
    public string SourceExpression { get; }

    public Result<IExpression> Operand { get; }

    public UnaryExpression(string sourceExpression, Result<IExpression> operand)
    {
        ArgumentGuard.IsNotNull(sourceExpression, nameof(sourceExpression));
        ArgumentGuard.IsNotNull(operand, nameof(operand));

        SourceExpression = sourceExpression;
        Operand = operand;
    }

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (Operand.Value is not null
            ? await Wrap(context, token).ConfigureAwait(false)
            : Result.FromExistingResult<bool>(Operand))
        .OnSuccess(result => Result.Success(!result.Value.IsTruthy()));

    private async Task<Result<bool>> Wrap(ExpressionEvaluatorContext context, CancellationToken token)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            return (await Operand.Value!.EvaluateAsync(context, token).ConfigureAwait(false)).TryCastAllowNull<bool>();
        }
        catch (Exception ex)
        {
            return Result.Error<bool>(ex, "Exception occured");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    public async Task<ExpressionParseResult> ParseAsync(CancellationToken token)
    {
        var operandResult = Operand.Value is not null
            ? await Operand.Value.ParseAsync(token).ConfigureAwait(false)
            : null;

        var result = new ExpressionParseResultBuilder()
            .WithExpressionComponentType(typeof(UnaryExpression))
            .WithSourceExpression(SourceExpression)
            .WithResultType(typeof(bool))
            .AddPartResult(operandResult ?? new ExpressionParseResultBuilder().FillFromResult(Operand), Constants.Operand)
            .SetStatusFromPartResults();

        return result;
    }

    public IEvaluatableBuilder ToBuilder() => Operand.ToEvaluatable().ToBuilder();
}
