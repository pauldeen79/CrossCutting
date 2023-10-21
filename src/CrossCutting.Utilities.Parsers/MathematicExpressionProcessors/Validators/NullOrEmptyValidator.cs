namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Validators;

internal sealed class NullOrEmptyValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (string.IsNullOrEmpty(state.Input))
        {
            return Result.NotFound<MathematicExpressionState>("Input cannot be null or empty");
        }

        return Result.Success(state);
    }
}
