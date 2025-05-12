namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Minute))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateMinuteProperty : IProperty
{
    public Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            return context.GetInstanceValueResult<DateTime>().Transform<object?>(result => result.Minute);
        });
}
