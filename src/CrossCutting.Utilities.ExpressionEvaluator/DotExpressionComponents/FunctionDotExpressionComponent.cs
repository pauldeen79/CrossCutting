namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class FunctionDotExpressionComponent : IDotExpressionComponent
{
    private readonly IMemberResolver _memberResolver;

    public FunctionDotExpressionComponent(IMemberResolver memberResolver)
    {
        ArgumentGuard.IsNotNull(memberResolver, nameof(memberResolver));

        _memberResolver = memberResolver;
    }

    public int Order => 21;

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        var context = new FunctionCallContext(state.FunctionParseResult.GetValueOrThrow()
            .Transform(x => x.ToBuilder()
                .WithMemberType(state.Type == DotExpressionType.Method
                    ? MemberType.Method
                    : MemberType.Property)
                .Build()), state.Context, state.Value);
        var result = _memberResolver.Resolve(context);
        if (result.Status == ResultStatus.NotFound)
        {
            return Result.Continue<object?>();
        }

        var member = result.GetValueOrThrow().Member;
        return member switch
        {
            IFunction function => function.Evaluate(context),
            _ => Result.Continue<object?>() //TODO: Review this path
        };
    }

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        var context = new FunctionCallContext(state.FunctionParseResult.GetValueOrThrow()
            .Transform(x => x.ToBuilder()
            .WithMemberType(state.Type == DotExpressionType.Method
                ? MemberType.Method
                : MemberType.Property)
            .Chain(x => x.Arguments.Insert(0, Constants.DotArgument)).Build()), state.Context, state.Value);
        var result = _memberResolver.Resolve(context);
        if (result.Status == ResultStatus.NotFound)
        {
            return Result.Continue<Type>();
        }

        if (!result.Status.IsSuccessful())
        {
            return Result.FromExistingResult<Type>(result);
        }

        //var member = result.Value?.Member;
        //return member switch
        //{
        //    IFunction => Result.Success(result.Value?.ReturnValueType!),
        //    _ => Result.Continue<Type>() //TODO: Review this path
        //};

        return Result.Success(result.Value?.ReturnValueType!);
    }
}
