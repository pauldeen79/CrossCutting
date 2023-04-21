namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Validators;

internal class EndWithOperatorValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (state.Input.TrimEnd().EndsWithAny(Operators.Aggregators.Select(x => x.Key.ToString())))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot end with an operator");
        }

        return Result<MathematicExpressionState>.Continue();
    }
}
