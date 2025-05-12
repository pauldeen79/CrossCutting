namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(bool))]
[MemberArgument(Constants.Expression, typeof(object))]
public class IsNullFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await context.GetArgumentValueResultAsync(0, Constants.Expression).ConfigureAwait(false))
            .Transform<object?>(result => result is null);
    }
}
