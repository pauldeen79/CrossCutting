namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class DateTimeExpressionParserProcessor : IExpression
{
    public int Order => 70;

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            not null when DateTime.TryParse(expression, formatProvider, DateTimeStyles.None, out var dt) => Result.Success<object?>(dt),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            not null when DateTime.TryParse(expression, formatProvider, DateTimeStyles.None, out _) => Result.Success(),
            _ => Result.Continue()
        };
}
