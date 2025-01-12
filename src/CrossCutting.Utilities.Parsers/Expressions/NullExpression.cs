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

    public Result Validate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            "null" => Result.Success(),
            _ => Result.Continue()
        };
}
