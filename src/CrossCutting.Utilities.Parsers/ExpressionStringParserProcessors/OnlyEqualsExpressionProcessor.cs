namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

internal class OnlyEqualsExpressionProcessor : IExpressionStringParserProcessor
{
    public Result<object> Process(ExpressionStringParserState state)
    {
        if (state.Input == "=")
        {
            return Result<object>.Success(state.Input);
        }

        return Result<object>.Continue();
    }
}
