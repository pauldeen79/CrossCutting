namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class UnaryExpression : IExpression<bool>
{
    private readonly ExpressionEvaluatorContext _context;

    public Result<IExpression> Operand { get; }

    public UnaryExpression(ExpressionEvaluatorContext context, Result<IExpression> operand)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(operand, nameof(operand));

        _context = context;
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
            .WithSourceExpression(_context.Expression)
            .WithResultType(typeof(bool))
            .AddPartResult(operandResult ?? new ExpressionParseResultBuilder().FillFromResult(Operand), Constants.Operand)
            .SetStatusFromPartResults();

        return result;
    }
}
