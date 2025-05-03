namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ObjectDotExpressionComponent : DotExpressionComponentBase<object>, IDynamicDescriptorsProvider, IMember
{
    private readonly MemberDescriptor _toStringDescriptor;

    public ObjectDotExpressionComponent(IMemberCallArgumentValidator validator, IMemberDescriptorCallback callback)
    {
        ArgumentGuard.IsNotNull(validator, nameof(validator));
        callback = ArgumentGuard.IsNotNull(callback, nameof(callback));

        _toStringDescriptor = callback.Map(EvaluateToString).GetValueOrThrow();

        Descriptor = new DotExpressionDescriptor<object>(new Dictionary<string, DotExpressionDelegates<object>>()
        {
            // Note that we might validate the number of arguments, in case of overloads. But just set to 0 so the function is always taken by name, regardless of number of arguments
            { nameof(ToString), new DotExpressionDelegates<object>(0, x => MemberCallArgumentValidator.Validate(x, validator, _toStringDescriptor), EvaluateToString) },
        });
    }

    public Result<IReadOnlyCollection<MemberDescriptor>> GetDescriptors(IMemberDescriptorCallback callback)
        => Result.Success<IReadOnlyCollection<MemberDescriptor>>([_toStringDescriptor]);

    public override int Order => 99;

    [MemberName(nameof(object.ToString))]
    [MemberInstanceType(typeof(object))]
    [MemberResultType(typeof(string))]
    private static Result<object?> EvaluateToString(DotExpressionComponentState state, object sourceValue)
        => Result.Success<object?>(sourceValue.ToString(state.Context.Settings.FormatProvider));
}
