namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Month))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateMonthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<DateTime>())
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.Instance).Month));
    }
}
