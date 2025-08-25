namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddMinutes))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(MinutesToAdd, typeof(int))]
public class DateTimeAddMinutesMethod : IMethod
{
    private const string MinutesToAdd = nameof(MinutesToAdd);

    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, context.GetInstanceValueResultAsync<DateTime>(token))
            .Add(MinutesToAdd, context.GetArgumentValueResultAsync<int>(0, "MinutesToAdd", token))
            .Build().ConfigureAwait(false))
            .OnSuccess<object?>(results => results.GetValue<DateTime>(Constants.Instance).AddMinutes(results.GetValue<int>(MinutesToAdd)));
    }
}
