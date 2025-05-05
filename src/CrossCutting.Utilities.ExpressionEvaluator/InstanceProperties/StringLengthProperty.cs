namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(string.Length))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(int))]
public class StringLengthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<string>().Transform<object?>(result => result.Length);
    }
}
