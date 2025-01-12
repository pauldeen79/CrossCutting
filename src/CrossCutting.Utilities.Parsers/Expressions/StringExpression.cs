namespace CrossCutting.Utilities.Parsers.Expressions;

public class StringExpression : IExpression
{
    public int Order => 40;

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
        => expression?.StartsWith("\"") switch
        {
            true when expression.EndsWith("\"") => Result.Success<object?>(expression.Substring(1, expression.Length - 2)),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string expression, IFormatProvider formatProvider, object? context)
        => expression?.StartsWith("\"") switch
        {
            true when expression.EndsWith("\"") => Result.Success(),
            _ => Result.Continue()
        };
}
