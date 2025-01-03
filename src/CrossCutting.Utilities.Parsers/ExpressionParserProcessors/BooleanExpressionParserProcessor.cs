namespace CrossCutting.Utilities.Parsers.ExpressionParserProcessors;

public class BooleanExpressionParserProcessor : IExpression
{
    public int Order => 10;

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            "true" => Result.Success<object?>(true),
            "false" => Result.Success<object?>(false),
            _ => Result.Continue<object?>()
        };

    public Result Validate(string expression, IFormatProvider formatProvider, object? context)
        => expression switch
        {
            "true" => Result.Success(),
            "false" => Result.Success(),
            _ => Result.Continue()
        };
}
