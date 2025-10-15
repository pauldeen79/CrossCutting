namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record ContextEvaluatable
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.State.TryGetValue(Constants.Context, out var value))
        {
            return await value.ConfigureAwait(false);
        }

        return Result.NoContent<object?>();
    }
}
