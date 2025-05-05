namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class StringExpressionComponent : IExpressionComponent
{
    public int Order => 11;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Expression.Length < 2 || !context.Expression.StartsWith("\"") || !context.Expression.EndsWith("\""))
        {
            return Result.Continue<object?>();
        }

        return Result.Success<object?>(context.Expression.Substring(1, context.Expression.Length - 2));
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
