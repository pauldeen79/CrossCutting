namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class StringExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 30;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        if (value.StartsWith("\"") && value.EndsWith("\""))
        {
            return Result<object?>.Success(value.Substring(1, value.Length - 2));
        }

        return Result<object?>.Continue();
    }
}
