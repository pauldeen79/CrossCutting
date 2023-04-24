namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class NullExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 20;

    public Result<object?> Parse(string value, IFormatProvider formatProvider)
    {
        if (value == "null")
        {
            return Result<object?>.Success(null);
        }

        return Result<object?>.Continue();
    }
}
