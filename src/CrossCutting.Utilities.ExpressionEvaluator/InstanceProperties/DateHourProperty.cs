namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(DateTime.Hour))]
[MemberInstanceType(typeof(DateTime))]
[MemberResultType(typeof(int))]
public class DateHourProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var instanceValueResult = context.GetInstanceValueResult<DateTime>();
        if (!instanceValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceValueResult);
        }

        return Result.Success<object?>(instanceValueResult.Value!.Hour);
    }
}
