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

    public async Task<Result<object?>> EvaluateAsync(CancellationToken token)
        => (await EvaluateTypedAsync(token).ConfigureAwait(false));

    public async Task<Result<bool>> EvaluateTypedAsync(CancellationToken token)
        => (Operand.Value is not null
            ? (await Operand.Value.EvaluateAsync(token).ConfigureAwait(false)).TryCastAllowNull<bool>()
            : Result.FromExistingResult<bool>(Operand))
        .OnSuccess(result => Result.Success(!result.Value.IsTruthy()));

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
