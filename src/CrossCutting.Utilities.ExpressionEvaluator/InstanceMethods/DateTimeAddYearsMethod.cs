namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddYears))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(YearsToAdd, typeof(int))]
public class DateTimeAddYearsMethod : IMethod
{
    private const string YearsToAdd = nameof(YearsToAdd);

    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, context.GetInstanceValueResultAsync<DateTime>())
            .Add(YearsToAdd, context.GetArgumentValueResultAsync<int>(0, "YearsToAdd"))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.Instance).AddYears(results.GetValue<int>(YearsToAdd))));
    }
}
