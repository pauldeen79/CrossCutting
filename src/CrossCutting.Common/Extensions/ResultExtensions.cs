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

    public static Task Either<T>(this T instance, Action<T> errorDelegate, Func<Result, Task<T>> successDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
            return Task.CompletedTask;
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

    public static Task Either<T>(this T instance, Action<T> errorDelegate, Func<Task<T>> successDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
            return Task.CompletedTask;
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

    public static Task<T> Either<T>(this T instance, Func<T, T> errorDelegate, Func<Result, Task<T>> successDelegate)
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

    public static T WhenNotSuccesful<T>(this T instance, Action<T> errorDelegate)
        where T : Result
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
        }

        return instance;
    }
}
