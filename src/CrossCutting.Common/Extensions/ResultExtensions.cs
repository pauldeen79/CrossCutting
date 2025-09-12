namespace CrossCutting.Common.Extensions;

public static class ResultExtensions
{
    private const string NullResultErrorMessage = "Result is null";

    public static void Either<T>(this T instance, Action<T> errorDelegate, Action<T> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
            return;
        }

        successDelegate(instance);
    }

    public static Task<T> Either<T>(this T instance, Action<T> errorDelegate, Func<T, Task<T>> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
            return Task.FromResult(instance);
        }

        return successDelegate(instance);
    }

    public static void Either<T>(this T instance, Action<T> errorDelegate, Action successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
            return;
        }

        successDelegate();
    }

    public static Task<T> Either<T>(this T instance, Action<T> errorDelegate, Func<Task<T>> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
            return Task.FromResult(instance);
        }

        return successDelegate();
    }

    public static T Either<T>(this T instance, Func<T, T> errorDelegate, Func<T, T> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return successDelegate(instance);
    }

    public static Task<T> Either<T>(this T instance, Func<T, T> errorDelegate, Func<T, Task<T>> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return Task.FromResult(errorDelegate(instance));
        }

        return successDelegate(instance);
    }

    public static T Either<T>(this T instance, Func<T, T> errorDelegate, Func<T> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return successDelegate();
    }

    public static Task<T> Either<T>(this T instance, Func<T, T> errorDelegate, Func<Task<T>> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return Task.FromResult(errorDelegate(instance));
        }

        return successDelegate();
    }

    public static T OnFailure<T>(this T instance, Action errorDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate();
        }

        return instance;
    }

    public static T OnFailure<T>(this T instance, Action<T> errorDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
        }

        return instance;
    }

    public static T OnFailure<T>(this T instance, Func<T> errorDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate();
        }

        return instance;
    }

    public static Task<T> OnFailure<T>(this T instance, Func<Task<T>> errorDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate();
        }

        return Task.FromResult(instance);
    }

    public static T OnFailure<T>(this T instance, Func<T, T> errorDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return instance;
    }

    public static Task<T> OnFailure<T>(this T instance, Func<T, Task<T>> errorDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(errorDelegate, nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return Task.FromResult(instance);
    }

    public static T OnSuccess<T>(this T instance, Action successDelegate)
    where T : Result
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            successDelegate();
        }

        return instance;
    }

    public static T OnSuccess<T>(this T instance, Action<T> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            successDelegate(instance);
        }

        return instance;
    }

    public static T OnSuccess<T>(this T instance, Func<T> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate();
        }

        return instance;
    }

    public static T OnSuccess<T>(this T instance, Func<T, T> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate(instance);
        }

        return instance;
    }

    public static Result<TTarget> OnSuccess<TTarget>(this Result instance, Func<Result, Result<TTarget>> successDelegate)
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate(instance);
        }

        return Result.FromExistingResult<TTarget>(instance);
    }

    public static Result<TTarget> OnSuccess<TSource, TTarget>(this Result<TSource> instance, Func<TSource, Result<TTarget>> successDelegate)
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        var result = instance.EnsureValue();
        if (result.IsSuccessful())
        {
            return successDelegate(result.Value!);
        }

        return Result.FromExistingResult<TTarget>(result);
    }

    public static Result<TTarget> OnSuccess<TSource, TTarget>(this Result<TSource> instance, Func<TSource, TTarget> successDelegate)
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        var result = instance.EnsureValue();
        if (result.IsSuccessful())
        {
            return Result.Success(successDelegate(result.Value!));
        }

        return Result.FromExistingResult<TTarget>(result);
    }

    public static Task<T> OnSuccessAsync<T>(this T instance, Func<Task<T>> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate();
        }

        return Task.FromResult(instance);
    }

    public static Task<T> OnSuccessAsync<T>(this T instance, Func<T, Task<T>> successDelegate)
        where T : Result
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate(instance);
        }

        return Task.FromResult(instance);
    }

    public static Task<Result<TTarget>> OnSuccessAsync<TTarget>(this Result instance, Func<Result, Task<Result<TTarget>>> successDelegate)
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate(instance);
        }

        return Task.FromResult(Result.FromExistingResult<TTarget>(instance));
    }

    public static Task<Result<TTarget>> OnSuccessAsync<TSource, TTarget>(this Result<TSource> instance, Func<TSource, Task<Result<TTarget>>> successDelegate)
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        var result = instance.EnsureValue();
        if (result.IsSuccessful())
        {
            return successDelegate(result.Value!);
        }

        return Task.FromResult(Result.FromExistingResult<TTarget>(result));
    }

    public static async Task<Result<TTarget>> OnSuccessAsync<TSource, TTarget>(this Result<TSource> instance, Func<TSource, Task<TTarget>> successDelegate)
    {
        ArgumentGuard.IsNotNull(successDelegate, nameof(successDelegate));

        var result = instance.EnsureValue();
        if (result.IsSuccessful())
        {
            return Result.Success(await successDelegate(result.Value!).ConfigureAwait(false));
        }

        return Result.FromExistingResult<TTarget>(result);
    }

    public static Result IgnoreNotFound(this Result instance)
    {
        if (instance.Status == ResultStatus.NotFound)
        {
            return Result.Continue();
        }

        return instance;
    }

    public static Result<T> IgnoreNotFound<T>(this Result<T> instance)
    {
        if (instance.Status == ResultStatus.NotFound)
        {
            return Result.Continue<T>();
        }

        return instance;
    }

    public static Result EnsureValue(this Result instance, string? errorMessage = null)
    {
        if (instance.IsSuccessful() && instance.GetValue() is null)
        {
            return Result.Error(errorMessage.WhenNullOrEmpty(() => "Result value is required"));
        }

        return instance;
    }

    public static Result<T> EnsureValue<T>(this Result<T> instance, string? errorMessage = null)
    {
        if (instance.IsSuccessful() && instance.Value is null)
        {
            return Result.Error<T>(errorMessage.WhenNullOrEmpty(() => "Result value is required"));
        }

        return instance;
    }

    public static Result Wrap(this Result instance, string errorMessage)
        => instance.IsSuccessful()
            ? instance
            : new Result(instance.Status, errorMessage, [], [instance], null);

    public static Result<T> Wrap<T>(this Result<T> instance, string errorMessage)
        => instance.IsSuccessful()
            ? instance
            : new Result<T>(instance.Value, instance.Status, errorMessage, [], [instance], null);

    public static Result EnsureNotNull(this Result instance, string? errorMessage = null)
    {
        if (instance is null)
        {
            return Result.Error(errorMessage ?? NullResultErrorMessage);
        }

        return instance;
    }

    public static Result<T> EnsureNotNull<T>(this Result<T> instance, string? errorMessage = null)
    {
        if (instance is null)
        {
            return Result.Error<T>(errorMessage ?? NullResultErrorMessage);
        }

        return instance;
    }

    public static Result WhenNull(this Result instance, ResultStatus status, string? errorMessage = null)
    {
        if (instance is null)
        {
            return status.ToResult(errorMessage ?? NullResultErrorMessage);
        }

        return instance;
    }

    public static Result<T> WhenNull<T>(this Result<T> instance, ResultStatus status, string? errorMessage = null)
    {
        if (instance is null)
        {
            return status.ToTypedResult(default(T), errorMessage ?? NullResultErrorMessage);
        }

        return instance;
    }

    public static T WhenNotContinue<T>(this IEnumerable<T> innerResults, Func<T> notFoundDelegate) where T : Result
    {
        ArgumentGuard.IsNotNull(innerResults, nameof(innerResults));
        ArgumentGuard.IsNotNull(notFoundDelegate, nameof(notFoundDelegate));

        foreach (var result in innerResults)
        {
            if (result.Status != ResultStatus.Continue)
            {
                return result;
            }
        }

        return notFoundDelegate();
    }
}
