namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(Array.Length))]
[MemberInstanceType(typeof(Array))]
[MemberResultType(typeof(int))]
public class ArrayLengthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<Array>().Transform<object?>(result => result.Length);
    }
}
