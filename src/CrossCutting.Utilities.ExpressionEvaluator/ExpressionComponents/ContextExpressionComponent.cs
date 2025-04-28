namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class ContextExpressionComponent : IExpressionComponent
{
    public int Order => 25;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var success = context.Context.TryGetValue(context.Expression, out var dlg);
        if (success)
        {
            return dlg!();
        }

        return Result.Continue<object?>();
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithSourceExpression(context.Expression)
            .WithExpressionComponentType(typeof(ContextExpressionComponent));

        var success = context.Context.TryGetValue(context.Expression, out var dlg);
        if (success)
        {
            return result.WithResultType(dlg!()?.Value?.GetType());
        }

        return result.WithStatus(ResultStatus.Continue);
    }
}
