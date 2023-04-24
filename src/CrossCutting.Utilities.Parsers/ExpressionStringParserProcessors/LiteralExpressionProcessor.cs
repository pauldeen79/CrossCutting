namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class LiteralExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 20;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        if (!state.Input.StartsWith("="))
        {
            return Result<object?>.Success(state.Input);
        }

        return Result<object?>.Continue();
    }
}
