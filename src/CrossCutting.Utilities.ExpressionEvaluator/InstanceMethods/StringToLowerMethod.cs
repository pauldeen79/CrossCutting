namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(string.ToLower))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToLowerMethod : IMethod
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        //TODO: Try adding Require or Validate method to dictionary builders, to get rid of argument guards, and make the entire code a one-liner (allowing expression body for methods)
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, context.GetInstanceValueResultAsync<string>(token))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => Result.Success<object?>(results.GetValue<string>(Constants.Instance).ToLower(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
