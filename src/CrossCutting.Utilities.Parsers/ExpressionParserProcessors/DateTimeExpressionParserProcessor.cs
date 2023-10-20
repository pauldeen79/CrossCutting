namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class DateTimeExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 70;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        if (DateTime.TryParse(value, formatProvider, DateTimeStyles.None, out var dt))
        {
            return Result.Success<object?>(dt);
        }

        return Result.Continue<object?>();
    }
}
