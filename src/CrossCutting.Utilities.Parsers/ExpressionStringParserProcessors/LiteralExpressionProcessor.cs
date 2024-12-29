namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class LiteralExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 200;

    public Result<object?> Process(ExpressionStringParserState state)
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

    public Result Validate(ExpressionStringParserState state)
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
