namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Year))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateYearProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var instanceResult = context.GetInstanceValueResult<DateTime>();
        if (!instanceResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceResult);
        }

        return Result.Success<object?>(instanceResult.GetValueOrThrow().Year);
    }
}
