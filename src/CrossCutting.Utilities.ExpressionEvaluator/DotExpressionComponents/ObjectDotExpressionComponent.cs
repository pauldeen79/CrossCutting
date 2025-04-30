namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class ObjectDotExpressionComponent : DotExpressionComponentBase<object>
{
    private static readonly FunctionDescriptor _toStringDescriptor = new FunctionDescriptorBuilder()
        .WithName(nameof(ToString))
        .WithFunctionType(typeof(StringDotExpressionComponent))
        .WithReturnValueType(typeof(string));

    public ObjectDotExpressionComponent(IFunctionCallArgumentValidator validator) : base(new DotExpressionDescriptor<object>(new Dictionary<string, DotExpressionDelegates<object>>()
    {
        // Note that we might validate the number of arguments, in case of overloads. But just set to 1 (the left part of the dot expression) so the function is always taken by name, regardless of number of arguments
        { nameof(ToString), new DotExpressionDelegates<object>(0, x => FunctionCallArgumentValidator.Validate(x, validator, _toStringDescriptor), EvaluateToString) },
    }))
    {
        ArgumentGuard.IsNotNull(validator, nameof(validator));
    }

    private static Result<object?> EvaluateToString(DotExpressionComponentState state, object sourceValue)
    {
        // Little hacking here... We need to add an 'instance' argument (sort of an extension method), to construct a FunctionCall from this DotExpression...
        var context = new FunctionCallContext(state.FunctionParseResult.Value!.ToBuilder().Chain(x => x.Arguments.Insert(0, Constants.DummyArgument)).Build(), state.Context);

        return new ResultDictionaryBuilder()
            .Add(Constants.Expression, () => Result.Success(sourceValue))
            .Build()
            .OnSuccess(results =>
                Result.Success<object?>(results.GetValue(Constants.Expression).ToString(context.Context.Settings.FormatProvider)));

    }

    public override int Order => 14;
}
