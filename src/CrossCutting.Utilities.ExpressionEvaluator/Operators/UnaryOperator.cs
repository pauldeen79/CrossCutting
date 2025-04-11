namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

internal sealed class UnaryOperator : IOperator
{
    public OperatorExpressionTokenType Operator { get; }
    public Result<IOperator> Operand { get; }

    public UnaryOperator(OperatorExpressionTokenType operatorType, Result<IOperator> operand)
    {
        Operator = operatorType;
        Operand = operand;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        var results = new ResultDictionaryBuilder<object?>()
            .Add(Constants.Expression, () => Operand.Value?.Evaluate(context) ?? Result.FromExistingResult<object?>(Operand))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return error;
        }

        return Result.Success<object?>(!results.GetValue(Constants.Expression).ToBoolean());
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        var operandResult = Operand.Value?.Parse(context);

        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(OperatorExpression))
            .WithSourceExpression(context.Expression)
            .WithResultType(typeof(bool))
            .AddPartResult(operandResult ?? new ExpressionParseResultBuilder().FillFromResult(Operand), Constants.Operand)
            .SetStatusFromPartResults();

        return result;
    }
}
