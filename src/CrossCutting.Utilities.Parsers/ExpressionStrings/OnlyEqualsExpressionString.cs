namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class OnlyEqualsExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input == "=")
        {
            return Result.Success<object?>(state.Input);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input == "=")
        {
            return Result.Success(typeof(string));
        }

        return Result.Continue<Type>();
    }
}
