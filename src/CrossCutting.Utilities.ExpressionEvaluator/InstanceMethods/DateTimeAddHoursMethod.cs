namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddHours))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(HoursToAdd, typeof(int))]
public class DateTimeAddHoursMethod : IMethod
{
    private const string HoursToAdd = nameof(HoursToAdd);
    
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<DateTime>())
            .Add(HoursToAdd, () => context.GetArgumentValueResult<int>(0, "HoursToAdd"))
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.Instance).AddHours(results.GetValue<int>(HoursToAdd))));
    }
}
