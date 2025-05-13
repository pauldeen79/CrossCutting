namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class StateExpressionComponent : IExpressionComponent
{
    public int Order => 25;

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var success = context.State.TryGetValue(context.Expression, out var dlg);
        if (success)
        {
            return await dlg.ConfigureAwait(false);
        }

        return Result.Continue<object?>();
    }

    public async Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithSourceExpression(context.Expression)
            .WithExpressionComponentType(typeof(StateExpressionComponent));

        var success = context.State.TryGetValue(context.Expression, out var dlg);
        if (success)
        {
            var stateResult = await dlg.ConfigureAwait(false);
            return result.WithResultType(stateResult.Value?.GetType());
        }

        return result.WithStatus(ResultStatus.Continue);
    }
}
