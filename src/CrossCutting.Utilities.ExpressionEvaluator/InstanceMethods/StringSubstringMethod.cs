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

    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResultAsync<string>(token))
            .Add(Index, () => context.FunctionCall.GetArgumentValueResultAsync<int>(0, Index, context, token))
            .Add(Length, () => context.FunctionCall.GetArgumentValueResultAsync<int?>(1, Length, context, null, token))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var sourceValue = results.GetValue<string>(Constants.Instance);
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
