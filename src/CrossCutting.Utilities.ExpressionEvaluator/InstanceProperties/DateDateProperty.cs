namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Date))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(DateTime))]
public class DateDateProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<DateTime>()
            .Transform(x => Result.Success<object?>(x.GetValueOrThrow().Date));
    }
}
