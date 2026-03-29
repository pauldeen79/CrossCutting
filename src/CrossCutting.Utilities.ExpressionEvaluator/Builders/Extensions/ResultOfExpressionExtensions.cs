namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions;

public static class ResultOfExpressionExtensions
{
    public static IEvaluatableBuilder ToEvaluatable(this Result<IExpression> expression)
        => (((IEvaluatable)expression.Value!) ?? new LiteralResultEvaluatable(Result.FromExistingResult<object?>(expression))).ToBuilder();
}
