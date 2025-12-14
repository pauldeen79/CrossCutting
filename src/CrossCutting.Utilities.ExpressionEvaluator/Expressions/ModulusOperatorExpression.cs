namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class ModulusOperatorExpression : BinaryOperatorExpressionBase
{
    public ModulusOperatorExpression(Result<IExpression> left, Result<IExpression> right, string sourceExpression) : base(left, right, sourceExpression)
    {
    }

    public override IEvaluatableBuilder ToBuilder()
        => new ModulusOperatorEvaluatableBuilder()
            .WithLeftOperand(Left.ToEvaluatable())
            .WithRightOperand(Right.ToEvaluatable());

    protected override Type? GetResultType(ExpressionParseResult? leftResult)
        => typeof(bool);

    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await(await EvaluateAsResultDictionaryAsync(context, token).ConfigureAwait(false))
            .OnSuccessAsync(results => new ModulusOperatorEvaluatable(new LiteralEvaluatable(results.GetValue(Constants.LeftExpression)), new LiteralEvaluatable(results.GetValue(Constants.RightExpression)))
            .EvaluateAsync(context, token)).ConfigureAwait(false);
}
