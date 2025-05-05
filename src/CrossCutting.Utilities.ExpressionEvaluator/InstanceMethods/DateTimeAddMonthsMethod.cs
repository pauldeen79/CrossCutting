namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddMonths))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(MonthsToAdd, typeof(int))]
public class DateTimeAddMonthsMethod : IMethod
{
    private const string MonthsToAdd = nameof(MonthsToAdd);

    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<DateTime>())
            .Add(MonthsToAdd, () => context.GetArgumentValueResult<int>(0, "MonthsToAdd"))
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.Instance).AddMonths(results.GetValue<int>(MonthsToAdd))));
    }
}
