namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Date))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
public class DateDateProperty : IProperty
{
    public Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            return context.GetInstanceValueResult<DateTime>().Transform<object?>(result => result.Date);
        });
}
