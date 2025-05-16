namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddSeconds))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(SecondsToAdd, typeof(int))]
public class DateTimeAddSecondsMethod : IMethod
{
    private const string SecondsToAdd = nameof(SecondsToAdd);

    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, context.GetInstanceValueResultAsync<DateTime>())
            .Add(SecondsToAdd, context.GetArgumentValueResultAsync<int>(0, "SecondsToAdd"))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.Instance).AddSeconds(results.GetValue<int>(SecondsToAdd))));
    }
}
