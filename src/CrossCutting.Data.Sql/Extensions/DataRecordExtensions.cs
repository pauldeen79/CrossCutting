using System;
using System.Data;
using System.Linq;

namespace CrossCutting.Data.Sql.Extensions
{
    public static class DataRecordExtensions
    {
        /// <summary>
        /// Determines whether the specified data reader has a specific column.
        /// </summary>
        /// <param name="dr">The data reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>
        ///   <c>true</c> if the specified dr has column; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasColumn(this IDataRecord dr, string columnName)
            => Enumerable
                .Range(0, dr.FieldCount)
                .Any(i => dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase));
    }
}
