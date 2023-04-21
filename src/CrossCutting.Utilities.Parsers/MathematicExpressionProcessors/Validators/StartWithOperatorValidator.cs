namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Validators;

internal class StartWithOperatorValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Input.TrimStart().StartsWithAny(Operators.Aggregators.Select(x => x.Key.ToString())))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot start with an operator");
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
