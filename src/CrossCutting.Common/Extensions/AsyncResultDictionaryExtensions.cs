namespace CrossCutting.Common.Extensions;

public static class AsyncResultDictionaryExtensions
{
    public static async Task<Result<T>> TryCastValueAsync<T>(this IReadOnlyDictionary<string, Task<Result<object?>>> instance, string resultKey)
        => instance.TryGetValue(resultKey, out var settingsResult)
            ? (await settingsResult.ConfigureAwait(false))
                .TryCastAllowNull<T>()
            : Result.NotFound<T>($"{resultKey} was not found in state");
}
