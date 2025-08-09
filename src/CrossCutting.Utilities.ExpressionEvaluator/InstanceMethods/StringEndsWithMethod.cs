namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(string.EndsWith))]
[MemberInstanceType(typeof(string))]
[MemberArgument(Constants.Expression, typeof(string))]
[MemberResultType(typeof(bool))]
public class StringEndsWithMethod : IMethod
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, context.GetInstanceValueResultAsync<string>(token))
            .Add(Constants.Expression, context.GetArgumentValueResultAsync<string>(0, Constants.Expression, token))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => Result.Success<object?>(results.GetValue<string>(Constants.Instance).EndsWith(results.GetValue<string>(Constants.Expression), context.Context.Settings.StringComparison)));
    }
}
