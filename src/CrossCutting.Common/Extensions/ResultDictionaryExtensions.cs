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

    public static Result Either<T>(this Dictionary<string, Result<T>> resultDictionary, Func<Dictionary<string, Result<T>>, Result> successDelegate, Func<Result, Result<T>> errorDelegate)
    {
        successDelegate = ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));
        errorDelegate = ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        var error = resultDictionary.GetError();

        return error switch
        {
            not null => errorDelegate(error),
            _ => successDelegate(resultDictionary)
        };
    }

    public static Result Either(this Dictionary<string, Result> resultDictionary, Func<Dictionary<string, Result>, Result> successDelegate, Func<Result, Result> errorDelegate)
    {
        successDelegate = ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));
        errorDelegate = ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        var error = resultDictionary.GetError();

        return error switch
        {
            not null => errorDelegate(error),
            _ => successDelegate(resultDictionary)
        };
    }

    public static Result<T> Either<T>(this Dictionary<string, Result> resultDictionary, Func<Dictionary<string, Result>, Result<T>> successDelegate, Func<Result, Result<T>> errorDelegate)
    {
        successDelegate = ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));
        errorDelegate = ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        var error = resultDictionary.GetError();

        return error switch
        {
            not null => errorDelegate(error),
            _ => successDelegate(resultDictionary)
        };
    }

    public static Result Either<T>(this Dictionary<string, Result<T>> resultDictionary, Action<Dictionary<string, Result<T>>> successDelegate, Func<Result, Result<T>> errorDelegate)
        => resultDictionary.Either(_ => { successDelegate(resultDictionary); return Result.Success(); }, errorDelegate);

    public static Result Either(this Dictionary<string, Result> resultDictionary, Action<Dictionary<string, Result>> successDelegate, Func<Result, Result> errorDelegate)
        => resultDictionary.Either(_ => { successDelegate(resultDictionary); return Result.Success(); }, errorDelegate);

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
}
