﻿namespace CrossCutting.Utilities.Parsers.MathematicExpressions;

public class Recursion : IMathematicExpression
{
    public Result<MathematicExpressionState> Evaluate(MathematicExpressionState state)
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

            if (!MathematicExpressionEvaluator.IsMathematicExpression(found))
            {
                // for now, we exit recursion if it's not a mathematical expression.
                // it's probably a function call like MYFUNCTION(bla)
                break;
            }

            var subResult = state.ParseDelegate(found, state.FormatProvider, state.Context);
            if (!subResult.IsSuccessful())
            {
                return Result.FromExistingResult<MathematicExpressionState>(subResult);
            }

            state.Remainder = state.Remainder.Substring(0, openIndex)
                + FormattableString.Invariant($"{MathematicExpressionEvaluator.TemporaryDelimiter}{state.Results.Count}{MathematicExpressionEvaluator.TemporaryDelimiter}")
                + state.Remainder.Substring(closeIndex + 1);

            state.Results.Add(subResult);

        } while (state.Remainder.IndexOf(")") > -1);

        return Result.Success(state);
    }
}
