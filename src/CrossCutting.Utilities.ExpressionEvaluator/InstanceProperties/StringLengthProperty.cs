namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(string.Length))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(int))]
public class StringLengthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var instanceValueResult = context.GetInstanceValueResult<string>();
        if (!instanceValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceValueResult);
        }

        return Result.Success<object?>(instanceValueResult.Value!.Length);
    }
}
