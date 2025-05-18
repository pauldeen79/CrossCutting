namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class MemberDotExpressionComponent : IDotExpressionComponent
{
    private readonly IMemberResolver _memberResolver;

    public MemberDotExpressionComponent(IMemberResolver memberResolver)
    {
        ArgumentGuard.IsNotNull(memberResolver, nameof(memberResolver));

        _memberResolver = memberResolver;
    }

    public int Order => 21;

    public async Task<Result<object?>> EvaluateAsync(DotExpressionComponentState state, CancellationToken token)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        var context = new FunctionCallContext(state);
        var result = (await _memberResolver.ResolveAsync(context, token).ConfigureAwait(false)).IgnoreNotFound();
        if (!result.IsSuccessful() || result.Value is null)
        {
            return result;
        }

        return result.Value.Member switch
        {
            INonGenericMember nonGenericMember => await nonGenericMember.EvaluateAsync(context, token).ConfigureAwait(false),
            _ => Result.NotSupported<object?>("Resolved member should be of type Method or Property")
        };
    }

    public async Task<Result<Type>> ValidateAsync(DotExpressionComponentState state, CancellationToken token)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        var context = new FunctionCallContext(state);

        return (await _memberResolver.ResolveAsync(context, token).ConfigureAwait(false))
            .IgnoreNotFound()
            .Transform(result => Result.Success(result.ReturnValueType!));
    }
}
