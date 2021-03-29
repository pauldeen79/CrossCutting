using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> instance, IEnumerable<T> itemsToAdd)
        {
            foreach (var itemToAdd in itemsToAdd)
            {
                instance.Add(itemToAdd);
            }

            return instance;
        }

        /// <summary>
        /// Fixes null reference on this enumerable instance, and optionally applies a filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> instance, Func<T, bool> predicate = null)
        {
            var notNull = instance ?? Array.Empty<T>();
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
        public static IEnumerable<T> DefaultWhenNull<T>(this IEnumerable<T> instance, IEnumerable<T> valueWhenNull = null)
            => instance ?? valueWhenNull ?? Array.Empty<T>();
    }
}
