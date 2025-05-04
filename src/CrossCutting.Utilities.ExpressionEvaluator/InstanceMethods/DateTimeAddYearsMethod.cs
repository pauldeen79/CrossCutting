namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddYears))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(YearsToAdd, typeof(int))]
public class DateTimeAddYearsMethod : IMethod
{
    private const string YearsToAdd = nameof(YearsToAdd);
    
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<DateTime>())
            .Add(YearsToAdd, () => context.GetArgumentValueResult<int>(0, "YearsToAdd"))
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.Instance).AddYears(results.GetValue<int>(YearsToAdd))));
    }
}
