namespace CrossCutting.Common.Extensions;

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
        var notNull = instance ?? Enumerable.Empty<T>();
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
        => instance ?? valueWhenNull ?? Enumerable.Empty<T>();

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
}
