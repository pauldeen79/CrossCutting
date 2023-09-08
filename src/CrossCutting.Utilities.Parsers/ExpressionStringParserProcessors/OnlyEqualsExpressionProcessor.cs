namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class OnlyEqualsExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 300;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input == "=")
        {
            return Result<object?>.Success(state.Input);
        }

        return Result<object?>.Continue();
    }
}
