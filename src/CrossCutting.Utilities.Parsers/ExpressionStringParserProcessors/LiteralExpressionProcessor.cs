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
            return Result<object?>.Success(state.Input.Substring(1));
        }

        if (!state.Input.StartsWith("="))
        {
            // literal
            return Result<object?>.Success(state.Input);
        }

        return Result<object?>.Continue();
    }
}
