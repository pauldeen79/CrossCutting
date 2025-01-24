namespace CrossCutting.Utilities.Parsers.Expressions;

public class BooleanExpression : IExpression
{
    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            "true" => Result.Success<object?>(true),
            "false" => Result.Success<object?>(false),
            _ => Result.Continue<object?>()
        };

    public Result<Type> Validate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            "true" => Result.Success(typeof(bool)),
            "false" => Result.Success(typeof(bool)),
            _ => Result.Continue<Type>()
        };
}
