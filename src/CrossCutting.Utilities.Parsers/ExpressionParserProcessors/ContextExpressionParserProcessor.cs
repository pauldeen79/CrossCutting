namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class ContextExpressionParserProcessor : IExpressionParserProcessor
{
    public int Order => 20;

    public Result<object?> Parse(string value, IFormatProvider formatProvider, object? context)
    {
        if (value == "context")
        {
            return Result.Success(context);
        }

        return Result.Continue<object?>();
    }

    public Result Validate(string value, IFormatProvider formatProvider, object? context)
    {
        if (value == "context")
        {
            return Result.Success();
        }

        // Other values are ignored, so the expression parser knows whether an expression is supported
        return Result.Continue();
    }
}
