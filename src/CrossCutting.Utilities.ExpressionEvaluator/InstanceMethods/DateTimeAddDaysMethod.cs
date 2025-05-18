namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddDays))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(DaysToAdd, typeof(int))]
public class DateTimeAddDaysMethod : IMethod
{
    private const string DaysToAdd = nameof(DaysToAdd);

    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, context.GetInstanceValueResultAsync<DateTime>(token))
            .Add(DaysToAdd, context.GetArgumentValueResultAsync<int>(0, "DaysToAdd", token))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.Instance).AddDays(results.GetValue<int>(DaysToAdd))));
    }
}
