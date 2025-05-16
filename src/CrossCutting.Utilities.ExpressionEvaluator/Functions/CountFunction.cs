namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(int))]
[MemberArgument("EnumerableExpression", typeof(IEnumerable))]
public class CountFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await context
            .GetArgumentValueResultAsync<IEnumerable>(0, "EnumerableExpression").ConfigureAwait(false))
            .Transform<object?>(enumerable => enumerable.OfType<object?>().Count());
    }
}
