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
            return Result<MathematicExpressionState>.NotSupported($"Input cannot contain {MathematicExpressionParser.TemporaryDelimiter}, as this is used internally for formatting");
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
