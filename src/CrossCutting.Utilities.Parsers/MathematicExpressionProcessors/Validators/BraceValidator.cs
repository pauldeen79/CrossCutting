namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Validators;

internal sealed class BraceValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        var index = -1;
        var bracketCount = 0;
        foreach (var character in state.Input)
        {
            index++;
            if (character == '(')
            {
                bracketCount += 1;
            }
            if (character == ')')
            {
                bracketCount--;
                if (bracketCount < 0)
                {
                    return Result<MathematicExpressionState>.NotFound($"Too many closing braces found");
                }
            }
        }

        if (bracketCount > 0)
        {
            var suffix = bracketCount > 1
                ? "s"
                : string.Empty;
            return Result<MathematicExpressionState>.NotFound($"Missing {bracketCount} close brace{suffix}");
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
