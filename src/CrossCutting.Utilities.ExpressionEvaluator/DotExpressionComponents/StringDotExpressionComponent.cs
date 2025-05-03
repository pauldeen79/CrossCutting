namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class StringDotExpressionComponent : DotExpressionComponentBase<string>, IDynamicDescriptorsProvider, IMember
{
    private readonly MemberDescriptor _substringDescriptor;

    private static readonly MemberDescriptor _lengthDescriptor = new MemberDescriptorBuilder()
        .WithName(Length)
        .WithMemberType(MemberType.Property)
        .WithInstanceType(typeof(string))
        .WithImplementationType(typeof(StringDotExpressionComponent))
        .WithReturnValueType(typeof(int));

    private const string Substring = nameof(Substring);
    private const string Index = nameof(Index);
    private const string Length = nameof(Length);

    public StringDotExpressionComponent(IMemberCallArgumentValidator validator, IMemberDescriptorCallback callback)
    {
        ArgumentGuard.IsNotNull(validator, nameof(validator));
        callback = ArgumentGuard.IsNotNull(callback, nameof(callback));

        _substringDescriptor = callback.Map(EvaluateSubstring).GetValueOrThrow();

        Descriptor = new DotExpressionDescriptor<string>(new Dictionary<string, DotExpressionDelegates<string>>()
        {
            { Length, new DotExpressionDelegates<string>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Length)) },
            // Note that we might validate the number of arguments, in case of overloads. But just set to 0 so the function is always taken by name, regardless of number of arguments
            { Substring, new DotExpressionDelegates<string>(0, x => MemberCallArgumentValidator.Validate(x, validator, _substringDescriptor), EvaluateSubstring) },
        });
    }

    public override int Order => 14;

    public Result<IReadOnlyCollection<MemberDescriptor>> GetDescriptors(IMemberDescriptorCallback callback)
        => Result.Success<IReadOnlyCollection<MemberDescriptor>>([_substringDescriptor, _lengthDescriptor]);

    [MemberName(nameof(string.Substring))]
    [MemberInstanceType(typeof(string))]
    [MemberResultType(typeof(string))]
    [MemberArgument(Index, typeof(int))]
    [MemberArgument(Length, typeof(int), false)]
    private static Result<object?> EvaluateSubstring(DotExpressionComponentState state, string sourceValue)
    {
        var context = new FunctionCallContext(state);

        return new ResultDictionaryBuilder()
            .Add(Index, () => context.FunctionCall.GetArgumentValueResult<int>(1, Index, context))
            .Add(Length, () => context.FunctionCall.GetArgumentValueResult<int?>(2, Length, context, null))
            .Build()
            .OnSuccess(results =>
            {
                var index = results.GetValue<int>(Index);
                var length = results.GetValue<int?>(Length);

                if (length is null)
                {
                    return sourceValue.Length >= index
                        ? Result.Success<object?>(sourceValue.Substring(index))
                        : Result.Invalid<object?>("Index must refer to a location within the string");
                }

                return sourceValue.Length >= index + length
                    ? Result.Success<object?>(sourceValue.Substring(index, length.Value))
                    : Result.Invalid<object?>("Index and length must refer to a location within the string");
            });
    }
}
