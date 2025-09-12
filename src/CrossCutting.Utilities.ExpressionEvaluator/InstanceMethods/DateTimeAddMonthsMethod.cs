namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddMonths))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(MonthsToAdd, typeof(int))]
public class DateTimeAddMonthsMethod : IMethod
{
    private const string MonthsToAdd = nameof(MonthsToAdd);

    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, context.GetInstanceValueResultAsync<DateTime>(token))
            .Add(MonthsToAdd, context.GetArgumentValueResultAsync<int>(0, "MonthsToAdd", token))
            .Build().ConfigureAwait(false))
            .OnSuccess<object?>(results => results.GetValue<DateTime>(Constants.Instance).AddMonths(results.GetValue<int>(MonthsToAdd)));
    }
}
