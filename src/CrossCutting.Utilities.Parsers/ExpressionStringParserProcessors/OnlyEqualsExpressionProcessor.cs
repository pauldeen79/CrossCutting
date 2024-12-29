namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class OnlyEqualsExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 300;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input == "=")
        {
            return Result.Success<object?>(state.Input);
        }

        return Result.Continue<object?>();
    }

    public Result Validate(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input == "=")
        {
            return Result.Success();
        }

        return Result.Continue();
    }
}
