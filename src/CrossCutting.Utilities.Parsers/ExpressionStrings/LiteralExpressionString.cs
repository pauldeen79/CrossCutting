namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class LiteralExpressionString : IExpressionString
{
    public int Order => 200;

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

    public Result Validate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input.StartsWith("\'="))
        {
            // escaped expression string
            return Result.Success();
        }

        if (!state.Input.StartsWith("="))
        {
            // literal
            return Result.Success();
        }

        return Result.Continue();
    }
}
