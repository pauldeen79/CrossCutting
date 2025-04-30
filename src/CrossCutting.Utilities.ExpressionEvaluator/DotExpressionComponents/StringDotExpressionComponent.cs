namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class StringDotExpressionComponent : DotExpressionComponentBase<string>
{
    private static readonly FunctionDescriptor _substringDescriptor = new FunctionDescriptorBuilder()
        .WithName(Substring)
        .WithFunctionType(typeof(StringDotExpressionComponent))
        .WithReturnValueType(typeof(string))
        .AddArguments
        (
            new FunctionDescriptorArgumentBuilder().WithName(StringExpression).WithType(typeof(string)).WithIsRequired(),
            new FunctionDescriptorArgumentBuilder().WithName(Index).WithType(typeof(int)).WithIsRequired(),
            new FunctionDescriptorArgumentBuilder().WithName(Length).WithType(typeof(int)).WithIsRequired()
        );

    private const string Substring = nameof(Substring);
    private const string StringExpression = nameof(StringExpression);
    private const string Index = nameof(Index);
    private const string Length = nameof(Length);

    public StringDotExpressionComponent(IFunctionCallArgumentValidator validator) : base(new DotExpressionDescriptor<string>(new Dictionary<string, DotExpressionDelegates<string>>()
    {
        { Length, new DotExpressionDelegates<string>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Length)) },
        // Note that we might validate the number of arguments, in case of overloads. But just set to 1 (the left part of the dot expression) so the function is always taken by name, regardless of number of arguments
        { Substring, new DotExpressionDelegates<string>(0, x => FunctionCallArgumentValidator.Validate(x, validator, _substringDescriptor), EvaluateSubstring) },
    }))
    {
        ArgumentGuard.IsNotNull(validator, nameof(validator));
    }

    private static Result<object?> EvaluateSubstring(DotExpressionComponentState state, string sourceValue)
    {
        // Little hacking here... We need to add an 'instance' argument (sort of an extension method), to construct a FunctionCall from this DotExpression...
        var context = new FunctionCallContext(state.FunctionParseResult.Value!.ToBuilder().Chain(x => x.Arguments.Insert(0, Constants.DummyArgument)).Build(), state.Context);

        return new ResultDictionaryBuilder()
            .Add(StringExpression, () => Result.Success(sourceValue))
            .Add(Index, () => context.FunctionCall.GetArgumentValueResult<int>(1, Index, context))
            .Add(Length, () => context.FunctionCall.GetArgumentValueResult<int?>(2, Length, context, null))
            .Build()
            .OnSuccess(results =>
            {
                var stringExpression = results.GetValue<string>(StringExpression);
                var index = results.GetValue<int>(Index);
                var length = results.GetValue<int?>(Length);

                if (length is null)
                {
                    return stringExpression.Length >= index
                        ? Result.Success<object?>(stringExpression.Substring(index))
                        : Result.Invalid<object?>("Index must refer to a location within the string");
                }

                return stringExpression.Length >= index + length
                    ? Result.Success<object?>(stringExpression.Substring(index, length.Value))
                    : Result.Invalid<object?>("Index and length must refer to a location within the string");
            });
    }

    public override int Order => 14;
}
