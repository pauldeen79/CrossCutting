namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(object.ToString))]
[MemberInstanceType(typeof(object))]
[MemberResultType(typeof(string))]
public class ObjectToStringMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var instanceResult = context.GetInstanceValueResult<object>();
        if (!instanceResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceResult);
        }

        var sourceValue = instanceResult.GetValueOrThrow();

        return Result.Success<object?>(sourceValue.ToString(context.Context.Settings.FormatProvider));
    }
}
