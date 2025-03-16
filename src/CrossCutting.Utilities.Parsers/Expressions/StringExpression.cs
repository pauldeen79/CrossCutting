namespace CrossCutting.Utilities.Parsers.Expressions;

public class StringExpression : IExpression
{
    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => expression?.StartsWith("\"") switch
        {
            true when expression.EndsWith("\"") => Result.Success<object?>(expression.Substring(1, expression.Length - 2)),
            _ => Result.Continue<object?>()
        };

    public Result<Type> Validate(ExpressionEvaluatorContext context)
        => expression?.StartsWith("\"") switch
        {
            true when expression.EndsWith("\"") => Result.Success(typeof(string)),
            _ => Result.Continue<Type>()
        };
}
