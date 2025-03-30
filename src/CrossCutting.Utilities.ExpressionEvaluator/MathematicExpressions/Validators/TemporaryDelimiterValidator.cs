namespace CrossCutting.Utilities.ExpressionEvaluator.MathematicExpressions.Validators;

internal sealed class TemporaryDelimiterValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Context.Expression.Contains(MathematicExpression.TemporaryDelimiter))
        {
            return Result.Invalid<MathematicExpressionState>($"Input cannot contain {MathematicExpression.TemporaryDelimiter}, as this is used internally for formatting");
        }

        return Result.Success(state);
    }
}
