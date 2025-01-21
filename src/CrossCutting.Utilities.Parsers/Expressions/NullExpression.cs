namespace CrossCutting.Utilities.Parsers.Expressions;

public class NullExpression : IExpression
{
    public int Order => 30;

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            "null" => Result.Success<object?>(null),
            _ => Result.Continue<object?>()
        };

    public Result<Type> Validate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            "null" => Result.NoContent<Type>(),
            _ => Result.Continue<Type>()
        };
}
