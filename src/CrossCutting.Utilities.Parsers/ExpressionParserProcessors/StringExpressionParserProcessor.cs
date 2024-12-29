namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class StringExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 40;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        if (value is null)
        {
            return Result.Continue<object?>();
        }

        if (value.StartsWith("\"") && value.EndsWith("\""))
        {
            return Result.Success<object?>(value.Substring(1, value.Length - 2));
        }

        return Result.Continue<object?>();
    }

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
    {
        if (value is null)
        {
            return Result.Continue();
        }

        if (value.StartsWith("\"") && value.EndsWith("\""))
        {
            return Result.Success();
        }

        // Other values are ignored, so the expression parser knows whether an expression is supported
        return Result.Continue();
    }
}
