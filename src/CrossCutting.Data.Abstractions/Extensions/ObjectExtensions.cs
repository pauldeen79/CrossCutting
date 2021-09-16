using System;

namespace CrossCutting.Data.Abstractions.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Fixes the db null value for using in a CLR object. (value received from database)
        /// </summary>
        /// <param name="value">The value.</param>
        public static object? FixDbNull(this object value)
            => value == DBNull.Value
                ? null
                : value;

        /// <summary>
        /// Fixes the null value for using in a DbCommand parameter.
        /// </summary>
        /// <param name="value">The value.</param>
        public static object FixNull(this object? value)
            => value ?? DBNull.Value;
    }
}
