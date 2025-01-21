namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class EmptyExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (string.IsNullOrEmpty(state.Input))
        {
            return Result.Success<object?>(string.Empty);
        }

        return Result.Continue<object?>();
    }

    public Result Validate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (string.IsNullOrEmpty(state.Input))
        {
            return Result.Success();
        }

        return Result.Continue();
    }
}
