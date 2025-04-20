namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class StringExpression : IExpression<string>
{
    public int Order => 11;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<string> EvaluateTyped(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Expression.Length < 2 || !context.Expression.StartsWith("\"") || !context.Expression.EndsWith("\""))
        {
            return Result.Continue<string>();
        }

        return Result.Success(context.Expression.Substring(1, context.Expression.Length - 2));
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithSourceExpression(context.Expression)
            .WithExpressionType(typeof(StringExpression))
            .WithResultType(typeof(string));

        if (context.Expression.Length < 2 || !context.Expression.StartsWith("\"") || !context.Expression.EndsWith("\""))
        {
            return result.WithStatus(ResultStatus.Continue);
        }

        return result.WithStatus(ResultStatus.Ok);
    }
}
