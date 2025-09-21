namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public partial record ContextExpression
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

    public override Task<ExpressionParseResult> ParseAsync(CancellationToken token)
        => Task.FromResult(new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.NotSupported)
            .WithExpressionComponentType(GetType())
            .Build());
}
