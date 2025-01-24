namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class LiteralExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input.StartsWith("\'="))
        {
            // escaped expression string
            return Result.Success<object?>(state.Input.Substring(1));
        }

        if (!state.Input.StartsWith("="))
        {
            // literal
            return Result.Success<object?>(state.Input);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input.StartsWith("\'="))
        {
            // escaped expression string
            return Result.Success(typeof(string));
        }

        if (!state.Input.StartsWith("="))
        {
            // literal
            return Result.Success(typeof(string));
        }

        return Result.Continue<Type>();
    }
}
