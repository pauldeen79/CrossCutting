namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Month))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateMonthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var instanceResult = context.GetInstanceValueResult<DateTime>();
        if (!instanceResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceResult);
        }

        return Result.Success<object?>(instanceResult.GetValueOrThrow().Month);
    }
}
