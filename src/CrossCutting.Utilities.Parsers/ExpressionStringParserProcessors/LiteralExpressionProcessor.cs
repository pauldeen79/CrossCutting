namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

internal class LiteralExpressionProcessor : IExpressionStringParserProcessor
{
    public Result<object> Process(ExpressionStringParserState state)
    {
        if (!state.Input.StartsWith("="))
        {
            return Result<object>.Success(state.Input);
        }

        return Result<object>.Continue();
    }
}
