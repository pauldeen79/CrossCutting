namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class StateExpressionComponent : IExpressionComponent
{
    public int Order => 25;

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var success = context.State.TryGetValue(context.Expression, out var dlg);
            if (success)
            {
                return dlg();
            }

            return Result.Continue<object?>();
        });

    public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context)
        => Task.Run<ExpressionParseResult>(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var result = new ExpressionParseResultBuilder()
                .WithSourceExpression(context.Expression)
                .WithExpressionComponentType(typeof(StateExpressionComponent));

            var success = context.State.TryGetValue(context.Expression, out var dlg);
            if (success)
            {
                return result.WithResultType(dlg().Value?.GetType());
            }

            return result.WithStatus(ResultStatus.Continue);
        });
}
