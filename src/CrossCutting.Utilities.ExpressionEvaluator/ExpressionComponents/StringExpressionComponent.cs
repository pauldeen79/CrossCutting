namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class StringExpressionComponent : IExpressionComponent<string>
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
            .WithExpressionComponentType(typeof(StringExpressionComponent))
            .WithResultType(typeof(string));

        if (context.Expression.Length < 2 || !context.Expression.StartsWith("\"") || !context.Expression.EndsWith("\""))
        {
            return result.WithStatus(ResultStatus.Continue);
        }

        return result.WithStatus(ResultStatus.Ok);
    }
}
