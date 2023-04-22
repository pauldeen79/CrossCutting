namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

internal class Recursion : IMathematicExpressionProcessor
{
    public Result<MathematicExpressionState> Process(MathematicExpressionState state)
    {
        do
        {
            var closeIndex = state.Input.IndexOf(")");
            if (closeIndex <= -1)
            {
                continue;
            }

            var openIndex = state.Input.LastIndexOf("(", closeIndex);
            var found = state.Remainder.Substring(openIndex + 1, closeIndex - openIndex - 1);
            var subResult = state.ParseDelegate(found, state.FormatProvider);
            if (!subResult.IsSuccessful())
            {
                return Result<MathematicExpressionState>.FromExistingResult(subResult);
            }

            state.Remainder = state.Remainder.Replace($"({found})", FormattableString.Invariant($"{MathematicExpressionParser.TemporaryDelimiter}{state.Results.Count}{MathematicExpressionParser.TemporaryDelimiter}"));
            state.Results.Add(subResult);

        } while (state.Remainder.IndexOf(")") > -1);

        return Result<MathematicExpressionState>.Success(state);
    }
}
