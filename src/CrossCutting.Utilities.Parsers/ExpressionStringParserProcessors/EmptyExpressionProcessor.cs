namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class EmptyExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 100;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (string.IsNullOrEmpty(state.Input))
        {
            return Result<object?>.Success(string.Empty);
        }

        return Result<object?>.Continue();
    }
}
