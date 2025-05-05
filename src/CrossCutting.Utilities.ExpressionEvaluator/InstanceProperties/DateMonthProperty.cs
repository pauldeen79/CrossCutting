namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Month))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateMonthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<DateTime>().Transform<object?>(result => result.Month);
    }
}
