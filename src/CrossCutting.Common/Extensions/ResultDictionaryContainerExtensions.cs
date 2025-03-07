namespace CrossCutting.Common.Extensions;

public static class ResultDictionaryContainerExtensions
{
    public static Result<T> GetError<T>(this IResultDictionaryContainer<T> instance)
        => instance.Results.GetError();

    public static Result GetError(this IResultDictionaryContainer instance)
        => instance.Results.GetError();

    public static Result OnSuccess<T>(this IResultDictionaryContainer<T> instance, Func<Dictionary<string, Result<T>>, Result> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Result OnSuccess(this IResultDictionaryContainer instance, Func<Dictionary<string, Result>, Result> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Result<T> OnSuccess<T>(this IResultDictionaryContainer instance, Func<Dictionary<string, Result>, Result<T>> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Result OnSuccess<T>(this IResultDictionaryContainer<T> instance, Action<Dictionary<string, Result<T>>> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Result OnSuccess(this IResultDictionaryContainer instance, Action<Dictionary<string, Result>> successDelegate)
        => instance.Results.OnSuccess(successDelegate);

    public static Result<T> OnFailure<T>(this IResultDictionaryContainer<T> instance, Action<Result<T>> errorDelegate)
        => instance.Results.OnFailure(errorDelegate).GetError();

    public static Result OnFailure(this IResultDictionaryContainer instance, Action<Result> errorDelegate)
        => instance.Results.OnFailure(errorDelegate).GetError();

    public static Dictionary<string, Result<T>> OnFailure<T>(this IResultDictionaryContainer<T> instance, Func<Result<T>, Result<T>> errorDelegate)
        => instance.Results.OnFailure(errorDelegate);

    public static Dictionary<string, Result> OnFailure(this IResultDictionaryContainer instance, Func<Result, Result> errorDelegate)
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
