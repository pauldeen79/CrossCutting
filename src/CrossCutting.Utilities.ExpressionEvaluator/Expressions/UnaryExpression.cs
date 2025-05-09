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

    public Result<object?> Evaluate()
        => EvaluateTyped().TryCastAllowNull<object?>();

    public Result<bool> EvaluateTyped()
        => (Operand.Value?.Evaluate().TryCastAllowNull<bool>() ?? Result.FromExistingResult<bool>(Operand))
            .OnSuccess(result => Result.Success(!result.Value.IsTruthy()));

    public ExpressionParseResult Parse()
    {
        var operandResult = Operand.Value?.Parse();

        var result = new ExpressionParseResultBuilder()
            .WithExpressionComponentType(typeof(UnaryExpression))
            .WithSourceExpression(_context.Expression)
            .WithResultType(typeof(bool))
            .AddPartResult(operandResult ?? new ExpressionParseResultBuilder().FillFromResult(Operand), Constants.Operand)
            .SetStatusFromPartResults();

        return result;
    }
}
