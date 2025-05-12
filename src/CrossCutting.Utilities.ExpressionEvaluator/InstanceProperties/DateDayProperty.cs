namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Day))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateDayProperty : IProperty
{
    public Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            return context.GetInstanceValueResult<DateTime>().Transform<object?>(result => result.Day);
        });
}
