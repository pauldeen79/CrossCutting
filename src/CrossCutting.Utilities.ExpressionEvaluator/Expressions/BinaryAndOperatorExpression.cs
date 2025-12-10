namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class BinaryAndOperatorExpression : BinaryOperatorExpressionBase, IExpression<bool>
{
    public BinaryAndOperatorExpression(Result<IExpression> left, Result<IExpression> right, string sourceExpression) : base(left, right, sourceExpression)
    {
    }

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await (await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new BinaryAndOperatorEvaluatable(results.GetEvaluatable(Constants.LeftExpression), results.GetEvaluatable(Constants.RightExpression))
            .EvaluateTypedAsync(context, token)).ConfigureAwait(false);

    public override IEvaluatableBuilder ToBuilder()
        => new BinaryAndOperatorEvaluatableBuilder()
            .WithLeftOperand(Left.ToEvaluatable())
            .WithRightOperand(Right.ToEvaluatable());

    protected override Type? GetResultType(ExpressionParseResult? leftResult)
        => typeof(bool);

    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await (await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new BinaryAndOperatorEvaluatable(results.GetEvaluatable(Constants.LeftExpression), results.GetEvaluatable(Constants.RightExpression))
            .EvaluateAsync(context, token)).ConfigureAwait(false);
}
