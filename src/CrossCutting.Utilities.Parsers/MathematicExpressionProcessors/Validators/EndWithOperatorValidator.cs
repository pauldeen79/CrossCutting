namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Validators;

internal sealed class EndWithOperatorValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Input.TrimEnd().EndsWithAny(MathematicOperators.Aggregators.Select(x => x.Character.ToString())))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot end with an operator");
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
