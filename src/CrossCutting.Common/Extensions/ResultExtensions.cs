namespace CrossCutting.Common.Extensions;

public static class ResultExtensions
{
    public static void Either<T>(this T instance, Action<T> errorDelegate, Action<T> successDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

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
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

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
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

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
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

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
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return successDelegate(instance);
    }

    public static Task<T> Either<T>(this T instance, Func<T, T> errorDelegate, Func<T, Task<T>> successDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return Task.FromResult(errorDelegate(instance));
        }

        return successDelegate(instance);
    }

    public static T Either<T>(this T instance, Func<T, T> errorDelegate, Func<T> successDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return successDelegate();
    }

    public static Task<T> Either<T>(this T instance, Func<T, T> errorDelegate, Func<Task<T>> successDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return Task.FromResult(errorDelegate(instance));
        }

        return successDelegate();
    }

    public static T OnFailure<T>(this T instance, Action errorDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate();
        }

        return instance;
    }

    public static T OnFailure<T>(this T instance, Action<T> errorDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
        }

        return instance;
    }

    public static T OnFailure<T>(this T instance, Func<T> errorDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate();
        }

        return instance;
    }

    public static Task<T> OnFailure<T>(this T instance, Func<Task<T>> errorDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate();
        }

        return Task.FromResult(instance);
    }

    public static T OnFailure<T>(this T instance, Func<T, T> errorDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return instance;
    }

    public static Task<T> OnFailure<T>(this T instance, Func<T, Task<T>> errorDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return Task.FromResult(instance);
    }

    public static T OnSuccess<T>(this T instance, Action successDelegate)
    where T : Result
    {
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            successDelegate();
        }

        return instance;
    }

    public static T OnSuccess<T>(this T instance, Action<T> successDelegate)
        where T : Result
    {
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            successDelegate(instance);
        }

        return instance;
    }

    public static T OnSuccess<T>(this T instance, Func<T> successDelegate)
        where T : Result
    {
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate();
        }

        return instance;
    }

    public static Task<T> OnSuccess<T>(this T instance, Func<Task<T>> successDelegate)
        where T : Result
    {
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate();
        }

        return Task.FromResult(instance);
    }

    public static T OnSuccess<T>(this T instance, Func<T, T> successDelegate)
        where T : Result
    {
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate(instance);
        }

        return instance;
    }

    public static Task<T> OnSuccess<T>(this T instance, Func<T, Task<T>> successDelegate)
        where T : Result
    {
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (instance.IsSuccessful())
        {
            return successDelegate(instance);
        }

        return Task.FromResult(instance);
    }
}
