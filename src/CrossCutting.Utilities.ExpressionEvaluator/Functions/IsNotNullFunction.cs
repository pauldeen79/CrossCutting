namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(bool))]
[MemberArgument(Constants.Expression, typeof(object))]
public class IsNotNullFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await context.GetArgumentValueResultAsync(0, Constants.Expression, token).ConfigureAwait(false))
            .Transform<object?>(result => result is not null);
    }
}
