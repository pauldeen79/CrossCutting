namespace CrossCutting.Utilities.ExpressionEvaluator.Mathematics.Validators;

internal sealed class BraceValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        var index = -1;
        var bracketCount = 0;
        foreach (var character in state.Context.Expression)
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
                    return Result.NotFound<MathematicExpressionState>($"Too many closing braces found");
                }
            }
        }

        if (bracketCount > 0)
        {
            var suffix = bracketCount > 1
                ? "s"
                : string.Empty;
            return Result.NotFound<MathematicExpressionState>($"Missing {bracketCount} close brace{suffix}");
        }

        return Result.Success(state);
    }
}
