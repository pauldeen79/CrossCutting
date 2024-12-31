namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class BooleanExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 10;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
        => value switch
        {
            "true" => Result.Success<object?>(true),
            "false" => Result.Success<object?>(false),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
        => value switch
        {
            "true" => Result.Success(),
            "false" => Result.Success(),
            _ => Result.Continue()
        };
}
