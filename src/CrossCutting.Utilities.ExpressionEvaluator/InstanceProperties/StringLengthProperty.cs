namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(string.Length))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(int))]
public class StringLengthProperty : IProperty
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<string>())
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<string>(Constants.Instance).Length));
    }
}
