namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class NullExpressionParserProcessor : IExpression
{
    public int Order => 30;

    public Result<object?> Evaluate(string value, IFormatProvider formatProvider, object? context)
        => value switch
        {
            "null" => Result.Success<object?>(null),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
        => value switch
        {
            "null" => Result.Success(),
            _ => Result.Continue()
        };
}
