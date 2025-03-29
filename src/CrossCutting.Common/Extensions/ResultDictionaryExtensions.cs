namespace CrossCutting.Common.Extensions;

public static class ResultDictionaryExtensions
{
    public static Result<T> GetError<T>(this Dictionary<string, Result<T>> resultDictionary)
        => resultDictionary.Select(x => x.Value).FirstOrDefault(x => !x.IsSuccessful());

    public static Result GetError(this Dictionary<string, Result> resultDictionary)
        => resultDictionary.Select(x => x.Value).FirstOrDefault(x => !x.IsSuccessful());

    public static Result OnSuccess<T>(this Dictionary<string, Result<T>> resultDictionary, Func<Dictionary<string, Result<T>>, Result> successDelegate)
    {
        successDelegate = ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        var error = resultDictionary.GetError();

        return error switch
        {
            not null => error,
            _ => successDelegate(resultDictionary)
        };
    }

    public static Result<T> OnSuccess<T>(this Dictionary<string, Result<T>> resultDictionary, Func<Dictionary<string, Result<T>>, Result<T>> successDelegate)
    {
        successDelegate = ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        var error = resultDictionary.GetError();

        return error switch
        {
            not null => error,
            _ => successDelegate(resultDictionary)
        };
    }

    public static Result OnSuccess(this Dictionary<string, Result> resultDictionary, Func<Dictionary<string, Result>, Result> successDelegate)
    {
        successDelegate = ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        var error = resultDictionary.GetError();

        return error switch
        {
            not null => error,
            _ => successDelegate(resultDictionary)
        };
    }

    public static Result<T> OnSuccess<T>(this Dictionary<string, Result> resultDictionary, Func<Dictionary<string, Result>, Result<T>> successDelegate)
    {
        successDelegate = ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        var error = resultDictionary.GetError();

        return error switch
        {
            not null => Result.FromExistingResult<T>(error),
            _ => successDelegate(resultDictionary)
        };
    }

    public static Result OnSuccess<T>(this Dictionary<string, Result<T>> resultDictionary, Action<Dictionary<string, Result<T>>> successDelegate)
        => resultDictionary.OnSuccess(_ => { successDelegate(resultDictionary); return Result.Success(); });

    public static Result OnSuccess(this Dictionary<string, Result> resultDictionary, Action<Dictionary<string, Result>> successDelegate)
        => resultDictionary.OnSuccess(_ => { successDelegate(resultDictionary); return Result.Success(); });

    public static Dictionary<string, Result<T>> OnFailure<T>(this Dictionary<string, Result<T>> resultDictionary, Action<Result<T>> errorDelegate)
    {
        errorDelegate = ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        var error = resultDictionary.GetError();
        if (error is not null)
        {
            errorDelegate(error);
        }

        return resultDictionary;
    }

    public static Dictionary<string, Result> OnFailure(this Dictionary<string, Result> resultDictionary, Action<Result> errorDelegate)
    {
        errorDelegate = ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        var error = resultDictionary.GetError();
        if (error is not null)
        {
            errorDelegate(error);
        }

        return resultDictionary;
    }

    public static Dictionary<string, Result<T>> OnFailure<T>(this Dictionary<string, Result<T>> resultDictionary, Func<Result<T>, Result<T>> errorDelegate)
    {
        errorDelegate = ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        var error = resultDictionary.Select(x => new { x.Key, x.Value }).FirstOrDefault(x => !x.Value.IsSuccessful());
        if (error is not null)
        {
            // note that for now, we take all success results until the first error, and then replace the error with the delegate result
            var result = new Dictionary<string, Result<T>>();
            result.AddRange(resultDictionary.TakeWhile(x => x.Value.IsSuccessful()));
            result.Add(error.Key, errorDelegate(error.Value));
            return result;
        }

        return resultDictionary;
    }

    public static Dictionary<string, Result> OnFailure(this Dictionary<string, Result> resultDictionary, Func<Result, Result> errorDelegate)
    {
        errorDelegate = ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        var error = resultDictionary.Select(x => new { x.Key, x.Value }).FirstOrDefault(x => !x.Value.IsSuccessful());
        if (error is not null)
        {
            // note that for now, we take all success results until the first error, and then replace the error with the delegate result
            var result = new Dictionary<string, Result>();
            result.AddRange(resultDictionary.TakeWhile(x => x.Value.IsSuccessful()));
            result.Add(error.Key, errorDelegate(error.Value));
            return result;
        }

        return resultDictionary;
    }

    public static object? GetValue(this Dictionary<string, Result> resultDictionary, string resultKey)
        => resultDictionary.TryGetValue(resultKey, out Result result)
            ? result.GetValue()
            : throw new ArgumentOutOfRangeException(nameof(resultKey), $"Unknown argument: {resultKey}");

    public static T GetValue<T>(this Dictionary<string, Result> resultDictionary, string resultKey)
        => resultDictionary.TryGetValue(resultKey, out Result result)
            ? result.CastValueAs<T>()
            : throw new ArgumentOutOfRangeException(nameof(resultKey), $"Unknown argument: {resultKey}");

    public static T GetValue<T>(this Dictionary<string, Result<T>> resultDictionary, string resultKey)
        => resultDictionary.TryGetValue(resultKey, out Result<T> result)
            ? result.Value!
            : throw new ArgumentOutOfRangeException(nameof(resultKey), $"Unknown argument: {resultKey}");

    public static T? TryGetValue<T>(this Dictionary<string, Result> resultDictionary, string resultKey)
        => resultDictionary.TryGetValue(resultKey, out Result result)
            ? result.TryCastValueAs<T>()
            : default;

    public static T? TryGetValue<T>(this Dictionary<string, Result<T>> resultDictionary, string resultKey)
        => resultDictionary.TryGetValue(resultKey, out Result<T> result)
            ? result.Value
            : default;

    public static T? TryGetValue<T>(this Dictionary<string, Result> resultDictionary, string resultKey, T? defaultValue)
        => resultDictionary.TryGetValue(resultKey, out Result result)
            ? result.TryCastValueAs(defaultValue)
            : defaultValue;

    public static T? TryGetValue<T>(this Dictionary<string, Result<T>> resultDictionary, string resultKey, T? defaultValue)
        => resultDictionary.TryGetValue(resultKey, out Result<T> result)
            ? result.Value
            : defaultValue;
}
