namespace CrossCutting.Utilities.Parsers.Expressions;

public class ContextExpression : IExpression
{
    public int Order => 20;

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            "context" => Result.Success(context),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            "context" => Result.Success(),
            _ => Result.Continue()
        };
}
