using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Common.Extensions
{
    public static class StringExtensions
    {
        private static readonly string[] _trueKeywords = new[] { "true", "t", "1", "y", "yes", "ja", "j" };
        private static readonly string[] _falseKeywords = new[] { "false", "f", "0", "n", "no", "nee" };

        /// <summary>
        /// Performs a is null or empty check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrEmpty">The when null or empty.</param>
        /// <returns></returns>
        public static string WhenNullOrEmpty(this string instance, string whenNullOrEmpty)
        {
            if (string.IsNullOrEmpty(instance))
            {
                return whenNullOrEmpty;
            }

            return instance;
        }

        /// <summary>
        /// Performs a is null or empty check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrEmptyDelegate">The delegate to invoke when null or empty.</param>
        /// <returns></returns>
        public static string WhenNullOrEmpty(this string instance, Func<string> whenNullOrEmptyDelegate)
        {
            if (string.IsNullOrEmpty(instance))
            {
                return whenNullOrEmptyDelegate();
            }

            return instance;
        }

        /// <summary>
        /// Performs a is null or whitespace check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrWhiteSpace">The when null or white space.</param>
        /// <returns></returns>
        public static string WhenNullOrWhitespace(this string instance, string whenNullOrWhiteSpace)
        {
            if (string.IsNullOrWhiteSpace(instance))
            {
                return whenNullOrWhiteSpace;
            }

            return instance;
        }

        /// <summary>
        /// Performs a is null or whitespace check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrWhiteSpace">The when null or white space.</param>
        /// <returns></returns>
        public static string WhenNullOrWhitespace(this string instance, Func<string> whenNullOrWhiteSpace)
        {
            if (string.IsNullOrWhiteSpace(instance))
            {
                return whenNullOrWhiteSpace();
            }

            return instance;
        }

        /// <summary>
        /// Determines whether the specified instance is true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsTrue(this string instance)
            => instance != null
                && _trueKeywords.Any(s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Determines whether the specified instance is false.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsFalse(this string instance)
            => instance != null
                && _falseKeywords.Any(s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, params string[] values) =>
            instance.StartsWithAny((IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, IEnumerable<string> values)
            => values.Any(v => instance.StartsWith(v));

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, StringComparison comparisonType, params string[] values)
            => instance.StartsWithAny(comparisonType, (IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, StringComparison comparisonType, IEnumerable<string> values)
            => values.Any(v => instance.StartsWith(v, comparisonType));

        /// <summary>
        /// Indicates whether the string instance ends with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, params string[] values)
            => instance.EndsWithAny((IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance ends with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, IEnumerable<string> values)
            => values.Any(v => instance.EndsWith(v));

        /// <summary>
        /// Indicates whether the string instance ends any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, StringComparison comparisonType, params string[] values)
            => instance.EndsWithAny(comparisonType, (IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance ends any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, StringComparison comparisonType, IEnumerable<string> values)
            => values.Any(v => instance.EndsWith(v, comparisonType));
    }
}
