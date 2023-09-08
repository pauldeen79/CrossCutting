namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

public class Recursion : IMathematicExpressionProcessor
{
    public Result<MathematicExpressionState> Process(MathematicExpressionState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        do
        {
            var closeIndex = state.Input.IndexOf(")");
            if (closeIndex <= -1)
            {
                continue;
            }

            var openIndex = state.Input.LastIndexOf("(", closeIndex);
            var found = state.Remainder.Substring(openIndex + 1, closeIndex - openIndex - 1);
            
            if (!MathematicExpressionParser.IsMathematicExpression(found))
            {
                // for now, we exit recursion if it's not a mathematical expression.
                // it's probably a function call like MYFUNCTION(bla)
                break;
            }
            
            var subResult = state.ParseDelegate(found, state.FormatProvider, state.Context);
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
