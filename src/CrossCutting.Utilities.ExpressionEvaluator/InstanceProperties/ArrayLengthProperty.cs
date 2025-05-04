namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(Array.Length))]
[MemberInstanceType(typeof(Array))]
[MemberResultType(typeof(int))]
public class ArrayLengthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<Array>()
            .Transform(x => Result.Success<object?>(x.GetValueOrThrow().Length));
    }
}
