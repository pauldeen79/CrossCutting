namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class UnaryExpression : IExpression
{
    public Result<IExpression> Operand { get; }

    public UnaryExpression(Result<IExpression> operand)
    {
        ArgumentGuard.IsNotNull(operand, nameof(operand));

        Operand = operand;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));

        return (Operand.Value?.Evaluate(context) ?? Result.FromExistingResult<object?>(Operand))
            .OnSuccess(result => Result.Success<object?>(!result.Value.IsTruthy()));
    }

    public Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context)
        => Evaluate(context).TryCastAllowNull<T>();

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var operandResult = Operand.Value?.Parse(context);

        var result = new ExpressionParseResultBuilder()
            .WithExpressionComponentType(typeof(UnaryExpression))
            .WithSourceExpression(context.Expression)
            .WithResultType(typeof(bool))
            .AddPartResult(operandResult ?? new ExpressionParseResultBuilder().FillFromResult(Operand), Constants.Operand)
            .SetStatusFromPartResults();

        return result;
    }
}
