namespace CrossCutting.Common.Extensions;

public static class ResultDictionaryContainerExtensions
{
    public static Result<T> GetError<T>(this IResultDictionaryContainer<T> instance)
        => instance.Results.GetError();

    public static Result GetError(this IResultDictionaryContainer instance)
        => instance.Results.GetError();

    public static Result OnSuccess<T>(this IResultDictionaryContainer<T> instance, Func<IReadOnlyDictionary<string, Result<T>>, Result> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Result OnSuccess(this IResultDictionaryContainer instance, Func<IReadOnlyDictionary<string, Result>, Result> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Result<T> OnSuccess<T>(this IResultDictionaryContainer instance, Func<IReadOnlyDictionary<string, Result>, Result<T>> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Result OnSuccess<T>(this IResultDictionaryContainer<T> instance, Action<IReadOnlyDictionary<string, Result<T>>> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Result OnSuccess(this IResultDictionaryContainer instance, Action<IReadOnlyDictionary<string, Result>> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Task<Result> OnSuccessAsync(this IResultDictionaryContainer instance, Func<IReadOnlyDictionary<string, Result>, Task<Result>> successDelegate)
        => instance.Results.OnSuccessAsync(successDelegate);

    public static Task<Result<T>> OnSuccessAsync<T>(this IResultDictionaryContainer instance, Func<IReadOnlyDictionary<string, Result>, Task<Result<T>>> successDelegate)
        => instance.Results.OnSuccessAsync(successDelegate);

    public static Result<T> OnFailure<T>(this IResultDictionaryContainer<T> instance, Action<Result<T>> errorDelegate)
        => instance.Results.OnFailure(errorDelegate).GetError();

    public static Result OnFailure(this IResultDictionaryContainer instance, Action<Result> errorDelegate)
        => instance.Results.OnFailure(errorDelegate).GetError();

    public static IReadOnlyDictionary<string, Result<T>> OnFailure<T>(this IResultDictionaryContainer<T> instance, Func<Result<T>, Result<T>> errorDelegate)
        => instance.Results.OnFailure(errorDelegate);

    public static IReadOnlyDictionary<string, Result> OnFailure(this IResultDictionaryContainer instance, Func<Result, Result> errorDelegate)
        => instance.Results.OnFailure(errorDelegate);

    public static object? GetValue(this IResultDictionaryContainer instance, string resultKey)
        => instance.Results.GetValue(resultKey);

    public static T? GetValue<T>(this IResultDictionaryContainer instance, string resultKey)
        => instance.Results.GetValue<T>(resultKey);

    public static T? TryGetValue<T>(this IResultDictionaryContainer instance, string resultKey)
        => instance.Results.TryGetValue<T>(resultKey);

    public static T? TryGetValue<T>(this IResultDictionaryContainer instance, string resultKey, T? defaultValue)
        => instance.Results.TryGetValue(resultKey, defaultValue);
}
