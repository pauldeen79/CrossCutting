namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class ContextExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 20;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
        => value switch
        {
            "context" => Result.Success(context),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
        => value switch
        {
            "context" => Result.Success(),
            _ => Result.Continue()
        };
}
