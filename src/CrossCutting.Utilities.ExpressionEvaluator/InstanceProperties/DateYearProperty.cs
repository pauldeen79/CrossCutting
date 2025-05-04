namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Year))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateYearProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<DateTime>()
            .Transform(x => Result.Success<object?>(x.GetValueOrThrow().Year));
    }
}
