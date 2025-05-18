namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(object.ToString))]
[MemberInstanceType(typeof(object))]
[MemberResultType(typeof(string))]
public class ObjectToStringMethod : IMethod
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await context.GetInstanceValueResultAsync<object?>(token).ConfigureAwait(false))
            .OnSuccess(x => Result.Success<object?>(x.Value.ToString(context.Context.Settings.FormatProvider)));
    }
}
