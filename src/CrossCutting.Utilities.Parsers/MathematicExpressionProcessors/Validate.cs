namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

internal class Validate : IMathematicExpressionProcessor
{
    public Result<MathematicExpressionState> Process(MathematicExpressionState state)
    {
        if (string.IsNullOrEmpty(state.Input))
        {
            return Result<MathematicExpressionState>.NotFound("Input cannot be null or empty");
        }

        if (state.Input.Contains(MathematicExpressionParser.TemporaryDelimiter))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot contain {MathematicExpressionParser.TemporaryDelimiter}, as this is used internally for formatting");
        }

        if (state.Input.TrimStart().StartsWithAny(Operators.Aggregators.Select(x => x.Key.ToString())))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot start with an operator");
        }

        if (state.Input.TrimEnd().EndsWithAny(Operators.Aggregators.Select(x => x.Key.ToString())))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot end with an operator");
        }

        if (Operators.Aggregators.Select(x => $"{x.Key}{x.Key}").Any(x => state.Input.Replace(" ", string.Empty).Contains(x)))
        {
            return Result<MathematicExpressionState>.NotFound($"Input cannot contain operators without values between them");
        }

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
                    return Result<MathematicExpressionState>.NotFound($"Too many closing brackets found");
                }
            }
        }

        if (bracketCount > 0)
        {
            var suffix = bracketCount > 1
                ? "s"
                : string.Empty;
            return Result<MathematicExpressionState>.NotFound($"Missing {bracketCount} close bracket{suffix}");
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
