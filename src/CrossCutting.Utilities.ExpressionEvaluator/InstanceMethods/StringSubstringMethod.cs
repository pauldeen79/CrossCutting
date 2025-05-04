namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(string.Substring))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
[MemberArgument(Index, typeof(int))]
[MemberArgument(Length, typeof(int), false)]
public class StringSubstringMethod : IMethod
{
    private const string Index = nameof(Index);
    private const string Length = nameof(Length);
    
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<string>())
            .Add(Index, () => context.FunctionCall.GetArgumentValueResult<int>(0, Index, context))
            .Add(Length, () => context.FunctionCall.GetArgumentValueResult<int?>(1, Length, context, null))
            .Build()
            .OnSuccess(results =>
            {
                var sourceValue = results.GetValue<string>(nameof(Constants.Instance));
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
