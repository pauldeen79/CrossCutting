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

        var instanceResult = context.GetInstanceValueResult<string>();
        if (!instanceResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceResult);
        }

        var sourceValue = instanceResult.GetValueOrThrow();

        return new ResultDictionaryBuilder()
            .Add(Index, () => context.FunctionCall.GetArgumentValueResult<int>(0, Index, context))
            .Add(Length, () => context.FunctionCall.GetArgumentValueResult<int?>(1, Length, context, null))
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
