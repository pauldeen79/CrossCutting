namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class BinaryOrOperatorExpression : BinaryOperatorExpressionBase, IExpression<bool>
{
    public BinaryOrOperatorExpression(Result<IExpression> left, Result<IExpression> right, string sourceExpression) : base(left, right, sourceExpression)
    {
    }

    public async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await (await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new BinaryOrOperatorEvaluatable(results.GetEvaluatable(Constants.LeftExpression), results.GetEvaluatable(Constants.RightExpression))
            .EvaluateTypedAsync(context, token)).ConfigureAwait(false);

    public override IEvaluatableBuilder ToBuilder()
        => new BinaryOrOperatorEvaluatableBuilder()
            .WithLeftOperand(Left.ToEvaluatable())
            .WithRightOperand(Right.ToEvaluatable());

    protected override Type? GetResultType(ExpressionParseResult? leftResult)
        => typeof(bool);

    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await (await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new BinaryOrOperatorEvaluatable(results.GetEvaluatable(Constants.LeftExpression), results.GetEvaluatable(Constants.RightExpression))
            .EvaluateAsync(context, token)).ConfigureAwait(false);
}
