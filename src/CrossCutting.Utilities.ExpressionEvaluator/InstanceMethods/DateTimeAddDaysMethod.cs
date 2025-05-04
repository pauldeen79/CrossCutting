namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddDays))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(DaysToAdd, typeof(int))]
public class DateTimeAddDaysMethod : IMethod
{
    private const string DaysToAdd = nameof(DaysToAdd);
    
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.DotArgument, () => context.GetInstanceValueResult<DateTime>())
            .Add(DaysToAdd, () => context.GetArgumentValueResult<int>(0, "DaysToAdd"))
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.DotArgument).AddDays(results.GetValue<int>(DaysToAdd))));
    }
}
