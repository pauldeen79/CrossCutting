namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ObjectDotExpressionComponent : DotExpressionComponentBase<object>
{
    private static readonly FunctionDescriptor _toStringDescriptor = new FunctionDescriptorBuilder()
        .WithName(nameof(ToString))
        .WithFunctionType(typeof(StringDotExpressionComponent))
        .WithReturnValueType(typeof(string));

    public ObjectDotExpressionComponent(IFunctionCallArgumentValidator validator) : base(new DotExpressionDescriptor<object>(new Dictionary<string, DotExpressionDelegates<object>>()
    {
        // Note that we might validate the number of arguments, in case of overloads. But just set to 0 so the function is always taken by name, regardless of number of arguments
        { nameof(ToString), new DotExpressionDelegates<object>(0, x => FunctionCallArgumentValidator.Validate(x, validator, _toStringDescriptor), EvaluateToString) },
    }))
    {
        ArgumentGuard.IsNotNull(validator, nameof(validator));
    }

    private static Result<object?> EvaluateToString(DotExpressionComponentState state, object sourceValue)
        => Result.Success<object?>(sourceValue.ToString(state.Context.Settings.FormatProvider));

    public override int Order => 99;
}
