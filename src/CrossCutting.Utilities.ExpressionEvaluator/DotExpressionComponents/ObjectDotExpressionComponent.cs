namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ObjectDotExpressionComponent : DotExpressionComponentBase<object>, IDynamicDescriptorsProvider, IMember
{
    private static readonly MemberDescriptor _toStringDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(ToString))
        .WithMemberType(MemberType.Method)
        .WithInstanceType(typeof(object))
        .WithImplementationType(typeof(ObjectDotExpressionComponent))
        .WithReturnValueType(typeof(string))
        .AddArguments(new MemberDescriptorArgumentBuilder().WithName(Constants.DotArgument).WithType(typeof(DateTime)).WithIsRequired());

    public ObjectDotExpressionComponent(IMemberCallArgumentValidator validator) : base(new DotExpressionDescriptor<object>(new Dictionary<string, DotExpressionDelegates<object>>()
    {
        // Note that we might validate the number of arguments, in case of overloads. But just set to 0 so the function is always taken by name, regardless of number of arguments
        { nameof(ToString), new DotExpressionDelegates<object>(0, x => MemberCallArgumentValidator.Validate(x, validator, _toStringDescriptor), EvaluateToString) },
    }))
    {
        ArgumentGuard.IsNotNull(validator, nameof(validator));
    }

    public IEnumerable<MemberDescriptor> GetDescriptors()
        => [_toStringDescriptor];

    public override int Order => 99;

    private static Result<object?> EvaluateToString(DotExpressionComponentState state, object sourceValue)
        => Result.Success<object?>(sourceValue.ToString(state.Context.Settings.FormatProvider));
}
