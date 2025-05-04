namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(DateTime.AddSeconds))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument(SecondsToAdd, typeof(int))]
public class DateTimeAddSecondsMethod : IMethod
{
    private const string SecondsToAdd = nameof(SecondsToAdd);
    
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.DotArgument, () => context.GetInstanceValueResult<DateTime>())
            .Add(SecondsToAdd, () => context.GetArgumentValueResult<int>(0, "SecondsToAdd"))
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.DotArgument).AddSeconds(results.GetValue<int>(SecondsToAdd))));
    }
}
