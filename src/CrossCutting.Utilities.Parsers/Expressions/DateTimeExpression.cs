namespace CrossCutting.Utilities.Parsers.Expressions;

public class DateTimeExpression : IExpression
{
    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            not null when DateTime.TryParse(expression, formatProvider, DateTimeStyles.None, out var dt) => Result.Success<object?>(dt),
            _ => Result.Continue<object?>()
        };

    public Result<Type> Validate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            not null when DateTime.TryParse(expression, formatProvider, DateTimeStyles.None, out _) => Result.Success(typeof(DateTime)),
            _ => Result.Continue<Type>()
        };
}
