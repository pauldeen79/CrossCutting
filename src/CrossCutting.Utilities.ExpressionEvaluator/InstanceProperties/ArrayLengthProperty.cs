namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(Array.Length))]
[MemberInstanceType(typeof(Array))]
[MemberResultType(typeof(int))]
public class ArrayLengthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var instanceValueResult = context.GetInstanceValueResult<Array>();
        if (!instanceValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceValueResult);
        }

        return Result.Success<object?>(instanceValueResult.Value!.Length);
    }
}
