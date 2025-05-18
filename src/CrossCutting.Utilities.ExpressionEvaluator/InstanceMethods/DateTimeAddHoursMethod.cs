namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddHours))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(HoursToAdd, typeof(int))]
public class DateTimeAddHoursMethod : IMethod
{
    private const string HoursToAdd = nameof(HoursToAdd);

    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, context.GetInstanceValueResultAsync<DateTime>(token))
            .Add(HoursToAdd, context.GetArgumentValueResultAsync<int>(0, "HoursToAdd", token))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.Instance).AddHours(results.GetValue<int>(HoursToAdd))));
    }
}
