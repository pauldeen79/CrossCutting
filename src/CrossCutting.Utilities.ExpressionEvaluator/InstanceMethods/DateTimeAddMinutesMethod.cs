namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddMinutes))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(MinutesToAdd, typeof(int))]
public class DateTimeAddMinutesMethod : IMethod
{
    private const string MinutesToAdd = nameof(MinutesToAdd);
    
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.DotArgument, () => context.GetInstanceValueResult<DateTime>())
            .Add(MinutesToAdd, () => context.GetArgumentValueResult<int>(0, "MinutesToAdd"))
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.DotArgument).AddMinutes(results.GetValue<int>(MinutesToAdd))));
    }
}
