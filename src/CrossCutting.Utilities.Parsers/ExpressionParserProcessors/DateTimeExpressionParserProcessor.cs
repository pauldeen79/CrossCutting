namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class DateTimeExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 70;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        if (value is not null && DateTime.TryParse(value, formatProvider, DateTimeStyles.None, out var dt))
        {
            return Result.Success<object?>(dt);
        }

        return Result.Continue<object?>();
    }

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
    {
        if (value is not null && DateTime.TryParse(value, formatProvider, DateTimeStyles.None, out _))
        {
            return Result.Success();
        }

        // Other values are ignored, so the expression parser knows whether an expression is supported
        return Result.Continue();
    }
}
