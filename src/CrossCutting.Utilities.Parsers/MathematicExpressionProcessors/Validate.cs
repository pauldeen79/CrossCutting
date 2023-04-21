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

        return Result<MathematicExpressionState>.Success(state);
    }
}
