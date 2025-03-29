namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class StringExpression : IExpression
{
    public int Order => 11;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression.StartsWith("\"") switch
        {
            true when context.Expression.EndsWith("\"") => Result.Success<object?>(context.Expression.Substring(1, context.Expression.Length - 2)),
            _ => Result.Continue<object?>()
        };
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithSourceExpression(context.Expression)
            .WithExpressionType(typeof(StringExpression));

        return context.Expression.StartsWith("\"") switch
        {
            true when context.Expression.EndsWith("\"") => result.WithStatus(ResultStatus.Ok).WithResultType(typeof(string)),
            _ => result.WithStatus(ResultStatus.Continue)
        };
    }
}
