namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class OnlyEqualsExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 30;

    public Result<object> Process(ExpressionStringParserState state)
    {
        if (state.Input == "=")
        {
            return Result<object>.Success(state.Input);
        }

        return Result<object>.Continue();
    }
}
