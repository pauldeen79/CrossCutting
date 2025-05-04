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

        var context = new FunctionCallContext(state.FunctionParseResult.GetValueOrThrow(), state.Context, state.Type.ToMemberType(), state.Value);
        var result = _memberResolver.Resolve(context);
        if (result.Status == ResultStatus.NotFound)
        {
            return Result.Continue<object?>();
        }

        var member = result.GetValueOrThrow().Member;
        return member switch
        {
            IMethod method => method.Evaluate(context),
            IProperty property => property.Evaluate(context),
            _ => Result.NotSupported<object?>("Resolved member should be of type Method or Property")
        };
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        var context = new FunctionCallContext(state.FunctionParseResult.GetValueOrThrow(), state.Context, state.Type.ToMemberType(), state.ResultType);

        var result = _memberResolver.Resolve(context);
        if (result.Status == ResultStatus.NotFound)
        {
            return Result.Continue<Type>();
        }

        if (!result.Status.IsSuccessful())
        {
            return Result.FromExistingResult<Type>(result);
        }

        return Result.Success(result.GetValueOrThrow().ReturnValueType!);
    }
}
