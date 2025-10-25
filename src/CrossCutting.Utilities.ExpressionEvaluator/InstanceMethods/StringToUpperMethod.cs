namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(string.ToUpper))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToUpperMethod : IMethod
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResultAsync<string>(token))
            .BuildAsync().ConfigureAwait(false))
            .OnSuccess<object?>(results => results.GetValue<string>(Constants.Instance).ToUpper(context.Context.Settings.FormatProvider.ToCultureInfo()));
    }
}
