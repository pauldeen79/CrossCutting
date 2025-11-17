namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(string.Contains))]
[MemberInstanceType(typeof(string))]
[MemberArgument(Constants.Expression, typeof(string))]
[MemberResultType(typeof(bool))]
public class StringContainsMethod : IMethod
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResultAsync<string>(token))
            .Add(Constants.Expression, () => context.GetArgumentValueResultAsync<string>(0, Constants.Expression, token))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess<object?>(results => results.GetValue<string>(Constants.Instance).IndexOf(results.GetValue<string>(Constants.Expression), context.Context.Settings.StringComparison) > -1);
    }
}
