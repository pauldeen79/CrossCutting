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

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        var context = new FunctionCallContext(state);
        return new ResultDictionaryBuilder()
            .Add("Resolve", () => _memberResolver.Resolve(context).IgnoreNotFound())
            .Build()
            .OnSuccess(results => results.GetValue<MemberAndTypeDescriptor>("Resolve").Member switch
            {
                IMethod method => method.Evaluate(context),
                IProperty property => property.Evaluate(context),
                _ => Result.NotSupported<object?>("Resolved member should be of type Method or Property")
            });
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        var context = new FunctionCallContext(state);

        return new ResultDictionaryBuilder()
            .Add("Resolve", () => _memberResolver.Resolve(context).IgnoreNotFound())
            .Build()
            .OnSuccess(results => Result.Success<Type>(results.GetValue<MemberAndTypeDescriptor>("Resolve").ReturnValueType!));
    }
}
