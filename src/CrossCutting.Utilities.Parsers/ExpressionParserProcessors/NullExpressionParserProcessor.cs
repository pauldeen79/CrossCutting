namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class NullExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 30;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        if (value == "null")
        {
            return Result.Success<object?>(null);
        }

        return Result.Continue<object?>();
    }
}
