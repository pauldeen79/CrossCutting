namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Month))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateMonthProperty : IProperty
{
    public Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            return context.GetInstanceValueResult<DateTime>().Transform<object?>(result => result.Month);
        });
}
