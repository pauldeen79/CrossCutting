namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Minute))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateMinuteProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<DateTime>().Transform<object?>(result => result.Minute);
    }
}
