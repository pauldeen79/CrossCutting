namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class UnaryNegateOperatorExpression : UnaryOperatorExpressionBase, IExpression<bool>
{
    public UnaryNegateOperatorExpression(Result<IExpression> operand, string sourceExpression) : base(operand, sourceExpression)
    {
    }

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await (await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new UnaryNegateOperatorEvaluatable(results.GetEvaluatable<bool>(Constants.Expression))
            .EvaluateTypedAsync(context, token)).ConfigureAwait(false);

    public override IEvaluatableBuilder ToBuilder()
        => ToTypedBuilder();

    protected override Type? GetResultType(ExpressionParseResult? leftResult)
        => typeof(bool);

    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await (await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new UnaryNegateOperatorEvaluatable(results.GetEvaluatable<bool>(Constants.Expression))
            .EvaluateAsync(context, token)).ConfigureAwait(false);


    IEvaluatableBuilder<bool> IEvaluatable<bool>.ToTypedBuilder()
        => ToTypedBuilder();

    private UnaryNegateOperatorEvaluatableBuilder ToTypedBuilder()
        => new UnaryNegateOperatorEvaluatableBuilder()
            .WithOperand(Operand.ToEvaluatable());
}
