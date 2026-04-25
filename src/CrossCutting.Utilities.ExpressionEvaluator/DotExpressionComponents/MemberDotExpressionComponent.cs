namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class MemberDotExpressionComponent(IMemberResolver memberResolver) : IDotExpressionComponent
{
    private readonly IMemberResolver _memberResolver = ArgumentGuard.IsNotNull(memberResolver, nameof(memberResolver));

    public int Order => 21;

    public async Task<Result<object?>> EvaluateAsync(DotExpressionComponentState state, CancellationToken token)
        => await Result.EnsureNotNull<object?>(state, nameof(state))
            .OnSuccessAsync(async () =>
            {
                var context = new FunctionCallContext(state);
                var result = (await _memberResolver.ResolveAsync(context, token).ConfigureAwait(false))
                    .EnsureNotNull("MemberResolver.ResolveAsync returned null")
                    .IgnoreNotFound();

                if (!result.IsSuccessful() || result.Value is null)
                {
                    return Result.FromExistingResult<object?>(result);
                }

                return result.Value.Member switch
                {
                    INonGenericMember nonGenericMember => await nonGenericMember.EvaluateAsync(context, token).ConfigureAwait(false),
                    _ => Result.NotSupported<object?>("Resolved member should be of type Method or Property")
                };
            }).ConfigureAwait(false);

    public async Task<Result<Type>> ValidateAsync(DotExpressionComponentState state, CancellationToken token)
        => await Result.EnsureNotNull<Type>(state, nameof(state))
            .OnSuccessAsync(async () =>
            {
                var context = new FunctionCallContext(state);

                return (await _memberResolver.ResolveAsync(context, token).ConfigureAwait(false))
                    .IgnoreNotFound()
                    .Transform(result => result.ReturnValueType!);
            }).ConfigureAwait(false);
}
