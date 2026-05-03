namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class SmallerOrEqualOperatorExpression : BinaryOperatorExpressionBase, IExpression<bool>
{
    public SmallerOrEqualOperatorExpression(Result<IExpression> left, Result<IExpression> right, string sourceExpression) : base(left, right, sourceExpression)
    {
    }

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await (await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new SmallerOrEqualOperatorEvaluatable(new LiteralEvaluatable(results.GetValue(Constants.LeftExpression)), new LiteralEvaluatable(results.GetValue(Constants.RightExpression)))
            .EvaluateTypedAsync(context, token)).ConfigureAwait(false);

    public override IEvaluatableBuilder ToBuilder()
        => ToTypedBuilder();

    protected override Type? GetResultType(ExpressionParseResult? leftResult)
        => typeof(bool);

    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await(await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new SmallerOrEqualOperatorEvaluatable(new LiteralEvaluatable(results.GetValue(Constants.LeftExpression)), new LiteralEvaluatable(results.GetValue(Constants.RightExpression)))
            .EvaluateAsync(context, token)).ConfigureAwait(false);

    IEvaluatableBuilder<bool> IEvaluatable<bool>.ToTypedBuilder()
        => ToTypedBuilder();

    private SmallerOrEqualOperatorEvaluatableBuilder ToTypedBuilder()
        => new SmallerOrEqualOperatorEvaluatableBuilder()
            .WithLeftOperand(Left.ToEvaluatable())
            .WithRightOperand(Right.ToEvaluatable());
}
