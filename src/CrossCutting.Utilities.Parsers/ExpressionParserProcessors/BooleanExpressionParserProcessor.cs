namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class BooleanExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 10;

    public Result<object> Parse(string value, IFormatProvider formatProvider)
    {
        if (value == "true")
        {
            return Result<object>.Success(true);
        }

        if (value == "false")
        {
            return Result<object>.Success(false);
        }

        return Result<object>.Continue();
    }
}
