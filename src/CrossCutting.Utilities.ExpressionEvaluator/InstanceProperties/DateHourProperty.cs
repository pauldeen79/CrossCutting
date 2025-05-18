namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Hour))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateHourProperty : IProperty
{
    public Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            return context.GetInstanceValueResult<DateTime>().Transform<object?>(result => result.Hour);
        }, token);
}
