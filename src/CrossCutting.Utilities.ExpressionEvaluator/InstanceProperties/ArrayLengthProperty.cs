namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(Array.Length))]
[MemberInstanceType(typeof(Array))]
[MemberResultType(typeof(int))]
public class ArrayLengthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var instanceResult = context.GetInstanceValueResult<Array>();
        if (!instanceResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceResult);
        }

        return Result.Success<object?>(instanceResult.GetValueOrThrow().Length);
    }
}
