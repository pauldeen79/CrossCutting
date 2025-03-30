namespace CrossCutting.Utilities.ExpressionEvaluator.MathematicExpressions;

public class Recursion : IMathematicExpression
{
    public Result<MathematicExpressionState> Evaluate(MathematicExpressionState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        do
        {
            var closeIndex = state.Context.Expression.IndexOf(")");
            if (closeIndex <= -1)
            {
                continue;
            }

            var openIndex = state.Context.Expression.LastIndexOf("(", closeIndex);
            var found = state.Remainder.Substring(openIndex + 1, closeIndex - openIndex - 1);

            if (!MathematicExpression.IsMathematicExpression(found))
            {
                // for now, we exit recursion if it's not a mathematical expression.
                // it's probably a function call like MYFUNCTION(bla)
                break;
            }

            var subResult = state.Context.Evaluate(found);
            if (!subResult.IsSuccessful())
            {
                return Result.FromExistingResult<MathematicExpressionState>(subResult);
            }

            state.Remainder = state.Remainder.Substring(0, openIndex)
                + FormattableString.Invariant($"{MathematicExpression.TemporaryDelimiter}{state.Results.Count}{MathematicExpression.TemporaryDelimiter}")
                + state.Remainder.Substring(closeIndex + 1);

            state.Results.Add(subResult);

        } while (state.Remainder.IndexOf(")") > -1);

        return Result.Success(state);
    }
}
