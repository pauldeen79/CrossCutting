namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class DateTimeExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 40;

    public Result<object> Parse(string value, IFormatProvider formatProvider)
    {
        if (DateTime.TryParse(value, formatProvider, DateTimeStyles.None, out var dt))
        {
            return Result<object>.Success(dt);
        }

        return Result<object>.Continue();
    }
}
