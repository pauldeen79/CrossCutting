namespace CrossCutting.Utilities.Parsers.MathematicExpressions.Validators;

internal sealed class TemporaryDelimiterValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Input.Contains(MathematicExpressionEvaluator.TemporaryDelimiter))
        {
            return Result.Invalid<MathematicExpressionState>($"Input cannot contain {MathematicExpressionEvaluator.TemporaryDelimiter}, as this is used internally for formatting");
        }

        return Result.Success(state);
    }
}
