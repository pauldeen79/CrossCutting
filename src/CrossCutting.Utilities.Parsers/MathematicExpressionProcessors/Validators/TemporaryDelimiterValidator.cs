namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Validators;

internal sealed class TemporaryDelimiterValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Input.Contains(MathematicExpressionParser.TemporaryDelimiter))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot contain {MathematicExpressionParser.TemporaryDelimiter}, as this is used internally for formatting");
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
