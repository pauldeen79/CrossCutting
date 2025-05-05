namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Date))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
public class DateDateProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<DateTime>())
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>(Constants.Instance).Date));
    }
}
