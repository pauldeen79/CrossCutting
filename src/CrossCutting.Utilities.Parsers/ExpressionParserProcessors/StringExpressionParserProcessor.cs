namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class StringExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 40;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
        => value?.StartsWith("\"") switch
        {
            true when value.EndsWith("\"") => Result.Success<object?>(value.Substring(1, value.Length - 2)),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
        => value?.StartsWith("\"") switch
        {
            true when value.EndsWith("\"") => Result.Success(),
            _ => Result.Continue()
        };
}
