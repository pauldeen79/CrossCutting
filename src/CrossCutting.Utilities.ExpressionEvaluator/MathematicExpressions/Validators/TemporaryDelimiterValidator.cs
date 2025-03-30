namespace CrossCutting.Utilities.ExpressionEvaluator.MathematicExpressions.Validators;

internal sealed class TemporaryDelimiterValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Context.Expression.Contains(MathematicExpressionEvaluator.TemporaryDelimiter))
        {
            return Result.Invalid<MathematicExpressionState>($"Input cannot contain {MathematicExpressionEvaluator.TemporaryDelimiter}, as this is used internally for formatting");
        }

        return Result.Success(state);
    }
}
