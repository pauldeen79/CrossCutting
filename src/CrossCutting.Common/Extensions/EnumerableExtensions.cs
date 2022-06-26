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
        return predicate == null
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

    public static TAccumulate AggregateUntil<TSource, TAccumulate>(
        this IEnumerable<TSource> source,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func,
        Func<TAccumulate, bool> predicate)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (func == null)
            throw new ArgumentNullException(nameof(func));

        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        var accumulate = seed;
        foreach (var item in source)
        {
            accumulate = func(accumulate, item);
            if (predicate(accumulate)) break;
        }
        return accumulate;
    }
}
