namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class BooleanExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 10;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        if (value == "true")
        {
            return Result.Success<object?>(true);
        }

        if (value == "false")
        {
            return Result.Success<object?>(false);
        }

        return Result.Continue<object?>();
    }

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
    {
        if (value == "true")
        {
            return Result.Success();
        }

        if (value == "false")
        {
            return Result.Success();
        }

        // Other values are ignored, so the expression parser knows whether an expression is supported
        return Result.Continue();
    }
}
