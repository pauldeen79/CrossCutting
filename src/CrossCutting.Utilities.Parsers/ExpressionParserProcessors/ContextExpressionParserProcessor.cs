namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class ContextExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 20;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        if (value == "context")
        {
            return Result.Success(context);
        }

        return Result.Continue<object?>();
    }
}
