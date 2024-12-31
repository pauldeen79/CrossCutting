namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class DateTimeExpressionParserProcessor : IExpression
{
    public int Order => 70;

    public Result<object?> Evaluate(string value, IFormatProvider formatProvider, object? context)
        => value switch
        {
            not null when DateTime.TryParse(value, formatProvider, DateTimeStyles.None, out var dt) => Result.Success<object?>(dt),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
        => value switch
        {
            not null when DateTime.TryParse(value, formatProvider, DateTimeStyles.None, out _) => Result.Success(),
            _ => Result.Continue()
        };
}
