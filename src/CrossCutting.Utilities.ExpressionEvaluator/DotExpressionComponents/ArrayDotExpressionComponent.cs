namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ArrayDotExpressionComponent : IDotExpressionComponent, IDynamicDescriptorsProvider, IMember
{
    private static readonly MemberDescriptor _lengthDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(Array.Length))
        .WithInstanceType(typeof(Array))
        .WithMemberType(MemberType.Property)
        .WithReturnValueType(typeof(int))
        .WithImplementationType(typeof(ArrayDotExpressionComponent));

    public int Order => 13;

    public Result<object?> Evaluate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Type == DotExpressionType.Property && state.Part == nameof(Array.Length) && state.CurrentEvaluateResult.Value!.GetType().IsArray)
        {
            return Result.Success<object?>(((Array)state.CurrentEvaluateResult.Value).Length);
        }

        return Result.Continue<object?>();
    }

    public IEnumerable<MemberDescriptor> GetDescriptors()
        => [_lengthDescriptor];

    public Result<Type> Validate(DotExpressionComponentState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Type == DotExpressionType.Property && state.Part == nameof(Array.Length) && state.ResultType!.IsArray)
        {
            return Result.Success(typeof(int));
        }

        return Result.Continue<Type>();
    }
}
