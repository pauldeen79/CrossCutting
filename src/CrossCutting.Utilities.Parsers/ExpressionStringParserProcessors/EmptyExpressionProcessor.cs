namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class EmptyExpressionProcessor : IExpressionStringParserProcessor
{
    public int Order => 10;

    public Result<object> Process(ExpressionStringParserState state)
    {
        if (string.IsNullOrEmpty(state.Input))
        {
            return Result<object>.Success(string.Empty);
        }

        return Result<object>.Continue();
    }
}
