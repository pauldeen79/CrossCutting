namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(string.ToLower))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToLowerMethod : IMethod
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, context.GetInstanceValueResultAsync<string>(token))
            .Build().ConfigureAwait(false))
            .OnSuccess<object?>(results => results.GetValue<string>(Constants.Instance).ToLower(context.Context.Settings.FormatProvider.ToCultureInfo()));
    }
}
