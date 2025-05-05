namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(Array.Length))]
[MemberInstanceType(typeof(Array))]
[MemberResultType(typeof(int))]
public class ArrayLengthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<Array>())
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<Array>(Constants.Instance).Length));
    }
}
