namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;

[MemberName(nameof(string.Length))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(int))]
public class StringLengthProperty : IProperty
{
    public Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            return context.GetInstanceValueResult<string>().Transform<object?>(result => result.Length);
        }, token);
}
