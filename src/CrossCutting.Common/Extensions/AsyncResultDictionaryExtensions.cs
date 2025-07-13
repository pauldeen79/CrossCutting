namespace CrossCutting.Common.Extensions;

public static class AsyncResultDictionaryExtensions
{
    public static async Task<Result<T>> TryCastValueAsync<T>(this IReadOnlyDictionary<string, Task<Result<object?>>> instance, string resultKey)
        => (await instance.TryGetValueAsync(resultKey).ConfigureAwait(false))
            .TryCastAllowNull<T>();

    public static async Task<Result<object?>> TryGetValueAsync(this IReadOnlyDictionary<string, Task<Result<object?>>> instance, string resultKey)
        => instance.TryGetValue(resultKey, out var settingsResult)
            ? (await settingsResult.ConfigureAwait(false))
            : Result.NotFound<object?>($"{resultKey} was not found in state");
}
