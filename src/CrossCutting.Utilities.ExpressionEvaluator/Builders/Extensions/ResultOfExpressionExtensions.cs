namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions;

public static class ResultOfExpressionExtensions
{
    public static IEvaluatable ToEvaluatable(this Result<IExpression> expression)
        => ((IEvaluatable)expression.Value!) ?? new LiteralResultEvaluatable(Result.FromExistingResult<IEvaluatable>(expression));
}
