namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Validators;

internal class EmptyValuePartValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (Operators.Aggregators.Select(x => $"{x.Character}{x.Character}").Any(x => state.Input.Replace(" ", string.Empty).Contains(x)))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot contain operators without values between them");
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
