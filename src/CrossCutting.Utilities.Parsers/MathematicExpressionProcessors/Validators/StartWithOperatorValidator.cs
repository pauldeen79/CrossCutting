namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Validators;

internal sealed class StartWithOperatorValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Input.TrimStart().StartsWithAny(MathematicOperators.Aggregators.Select(x => x.Character.ToString())))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot start with an operator");
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
