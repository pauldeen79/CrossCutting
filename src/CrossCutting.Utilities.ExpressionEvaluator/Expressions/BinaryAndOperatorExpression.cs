namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class BinaryAndOperatorExpression : BinaryOperatorExpressionBase, IExpression<bool>
{
    public BinaryAndOperatorExpression(Result<IExpression> left, Result<IExpression> right, string sourceExpression) : base(left, right, sourceExpression)
    {
    }

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await (await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new BinaryAndOperatorEvaluatable(new LiteralEvaluatable<bool>(results.GetValue<bool>(Constants.LeftExpression)), new LiteralEvaluatable<bool>(results.GetValue<bool>(Constants.RightExpression)))
            .EvaluateTypedAsync(context, token)).ConfigureAwait(false);

    public override IEvaluatableBuilder ToBuilder()
        => new BinaryAndOperatorEvaluatableBuilder()
            .With(x => x.LeftOperand = Left.Value!)
            .With(x => x.RightOperand = Right.Value!);

    protected override Type? GetResultType(ExpressionParseResult? leftResult)
        => typeof(bool);

    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await(await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new BinaryAndOperatorEvaluatable(new LiteralEvaluatable<bool>(results.GetValue<bool>(Constants.LeftExpression)), new LiteralEvaluatable<bool>(results.GetValue<bool>(Constants.RightExpression)))
            .EvaluateAsync(context, token)).ConfigureAwait(false);
}
