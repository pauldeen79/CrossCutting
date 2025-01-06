namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class OnlyEqualsExpressionString : IExpressionString
{
    public int Order => 300;

    public Result<object?> Evaluate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input == "=")
        {
            return Result.Success<object?>(state.Input);
        }

        return Result.Continue<object?>();
    }

    public Result Validate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input == "=")
        {
            return Result.Success();
        }

        return Result.Continue();
    }
}
