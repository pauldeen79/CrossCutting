﻿namespace CrossCutting.Common.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// Fixes null reference on this enumerable instance, and optionally applies a filter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T>? instance, Func<T, bool>? predicate = null)
    {
        var notNull = instance ?? [];
        return predicate is null
            ? notNull
            : notNull.Where(predicate);
    }

    /// <summary>
    /// Gets the value of an IEnumerable, or a default value when it's empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="valueWhenNull">The value when null. If this is null, an empty array will be used.</param>
    /// <returns>
    /// Typed array.
    /// </returns>
    public static IEnumerable<T> DefaultWhenNull<T>(this IEnumerable<T>? instance, IEnumerable<T>? valueWhenNull = null)
        => instance ?? valueWhenNull ?? [];

    public static IEnumerable<T> TakeWhileWithFirstNonMatching<T>(this IEnumerable<T> instance, Func<T, bool> predicate)
    {
        ArgumentGuard.IsNotNull(predicate, nameof(predicate));

        foreach (var item in instance)
        {
            yield return item;
            if (!predicate(item))
            {
                break;
            }
        }
    }

    public static IEnumerable<T> WhenEmpty<T>(this IEnumerable<T> instance, IEnumerable<T> whenEmpty)
        => instance.Any()
            ? instance
            : whenEmpty;

    public static IEnumerable<T> WhenEmpty<T>(this IEnumerable<T> instance, Func<IEnumerable<T>> whenEmpty)
    {
        ArgumentGuard.IsNotNull(whenEmpty, nameof(whenEmpty));

        return instance.Any()
            ? instance
            : whenEmpty.Invoke();
    }

    public static TResult Pipe<TProcessor, TResult>(this IEnumerable<TProcessor> processors, TResult seed, Func<TResult, TProcessor, TResult> processDelegate, Func<TResult, bool> predicate, Func<TResult, TResult>? defaultDelegate = null)
    {
        ArgumentGuard.IsNotNull(processDelegate, nameof(processDelegate));
        ArgumentGuard.IsNotNull(predicate, nameof(predicate));

        var result = seed;
        foreach (var processor in processors)
        {
            result = processDelegate.Invoke(result, processor);
            if (!predicate(result))
            {
                return result;
            }
        }

        return defaultDelegate is null
            ? result
            : defaultDelegate(result);
    }

    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(
        this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method,
        int concurrency = int.MaxValue)
    {
        var semaphore = new SemaphoreSlim(concurrency);
        try
        {
            return await Task.WhenAll(source.Select(async s =>
            {
                try
                {
                    await semaphore.WaitAsync().ConfigureAwait(false);
                    return await method(s).ConfigureAwait(false);
                }
                finally
                {
                    semaphore.Release();
                }
            })).ConfigureAwait(false);
        }
        finally
        {
            semaphore.Dispose();
        }
    }

    public static Result PerformUntilFailure<T>(this IEnumerable<T> instance, Func<T, Result> actionDelegate)
        => instance.PerformUntilFailure(Result.Success, actionDelegate);

    public static Result PerformUntilFailure<T>(this IEnumerable<T> instance, Func<Result> defaultValueDelegate, Func<T, Result> actionDelegate)
    {
        ArgumentGuard.IsNotNull(defaultValueDelegate, nameof(defaultValueDelegate));
        ArgumentGuard.IsNotNull(actionDelegate, nameof(actionDelegate));

        return instance
            .Select(x => actionDelegate(x))
            .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
            .LastOrDefault() ?? defaultValueDelegate();
    }

    public static Task<Result> PerformUntilFailure<T>(this IEnumerable<T> instance, Func<T, Task<Result>> actionDelegate)
        => instance.PerformUntilFailure(Result.Success, actionDelegate);

    public static async Task<Result> PerformUntilFailure<T>(this IEnumerable<T> instance, Func<Result> defaultValueDelegate, Func<T, Task<Result>> actionDelegate)
    {
        ArgumentGuard.IsNotNull(defaultValueDelegate, nameof(defaultValueDelegate));
        ArgumentGuard.IsNotNull(actionDelegate, nameof(actionDelegate));

        foreach (var item in instance)
        {
            var result = await actionDelegate(item).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return defaultValueDelegate();
    }

    public static Task<Result> PerformUntilFailure(this IEnumerable instance, Func<object, Task<Result>> actionDelegate)
        => instance.PerformUntilFailure(Result.Success, actionDelegate);

    public static async Task<Result> PerformUntilFailure(this IEnumerable instance, Func<Result> defaultValueDelegate, Func<object, Task<Result>> actionDelegate)
    {
        ArgumentGuard.IsNotNull(defaultValueDelegate, nameof(defaultValueDelegate));
        ArgumentGuard.IsNotNull(actionDelegate, nameof(actionDelegate));

        foreach (var item in instance)
        {
            var result = await actionDelegate(item).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return defaultValueDelegate();
    }
}
