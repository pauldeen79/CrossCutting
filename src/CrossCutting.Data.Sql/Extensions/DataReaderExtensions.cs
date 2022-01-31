namespace CrossCutting.Data.Sql.Extensions;

public static class DataReaderExtensions
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static T GetValue<T>(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
#pragma warning disable CS8604 // Possible null reference argument.
            => instance.GetValue<T>(columnName, default, skipUnknownColumn);
#pragma warning restore CS8604 // Possible null reference argument.

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static T GetValue<T>(this IDataReader instance, string columnName, T valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => (T)instance.GetValue(ordinal));

    /// <summary>
    /// Determines whether the specified instance is null.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <returns>
    /// true when null, otherwise false.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static bool IsDBNull(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, false, false, (reader, ordinal) => reader.IsDBNull(ordinal));

    /// <summary>
    /// Gets the boolean value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static bool GetBoolean(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, true, (reader, ordinal) => reader.GetBoolean(ordinal));

    /// <summary>
    /// Gets the boolean value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static bool GetBoolean(this IDataReader instance, string columnName, bool valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetBoolean(ordinal));

    /// <summary>
    /// Gets the nullable boolean value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static bool? GetNullableBoolean(this IDataReader instance, string columnName, bool? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetBoolean(ordinal));

    /// <summary>
    /// Gets the byte value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static byte GetByte(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, true, (reader, ordinal) => reader.GetByte(ordinal));

    /// <summary>
    /// Gets the byte value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static byte GetByte(this IDataReader instance, string columnName, byte valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetByte(ordinal));

    /// <summary>
    /// Gets the nullable byte value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static byte? GetNullableByte(this IDataReader instance, string columnName, byte? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetByte(ordinal));

    /// <summary>
    /// Gets the datetime value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static DateTime GetDateTime(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, true, (reader, ordinal) => reader.GetDateTime(ordinal));

    /// <summary>
    /// Gets the datetime value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static DateTime GetDateTime(this IDataReader instance, string columnName, DateTime valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetDateTime(ordinal));

    /// <summary>
    /// Gets the nullable datetime value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static DateTime? GetNullableDateTime(this IDataReader instance, string columnName, DateTime? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetDateTime(ordinal));

    /// <summary>
    /// Gets the decimal value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static decimal GetDecimal(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, true, (reader, ordinal) => reader.GetDecimal(ordinal));

    /// <summary>
    /// Gets the decimal value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static decimal GetDecimal(this IDataReader instance, string columnName, decimal valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetDecimal(ordinal));

    /// <summary>
    /// Gets the nullable decimal value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static decimal? GetNullableDecimal(this IDataReader instance, string columnName, decimal? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetDecimal(ordinal));

    /// <summary>
    /// Gets the double value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static double GetDouble(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, true, (reader, ordinal) => reader.GetDouble(ordinal));

    /// <summary>
    /// Gets the double value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static double GetDouble(this IDataReader instance, string columnName, double valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetDouble(ordinal));

    /// <summary>
    /// Gets the nullable double value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static double? GetNullableDouble(this IDataReader instance, string columnName, double? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetDouble(ordinal));

    /// <summary>
    /// Gets the float value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static float GetFloat(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, true, (reader, ordinal) => reader.GetFloat(ordinal));

    /// <summary>
    /// Gets the float value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static float GetFloat(this IDataReader instance, string columnName, float valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetFloat(ordinal));

    /// <summary>
    /// Gets the nullable float value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static float? GetNullableFloat(this IDataReader instance, string columnName, float? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetFloat(ordinal));

    /// <summary>
    /// Gets the Guid value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static Guid GetGuid(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, Guid.Empty, true, true, (reader, ordinal) => reader.GetGuid(ordinal));

    /// <summary>
    /// Gets the Guid value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static Guid GetGuid(this IDataReader instance, string columnName, Guid valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetGuid(ordinal));

    /// <summary>
    /// Gets the nullable Guid value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static Guid? GetNullableGuid(this IDataReader instance, string columnName, Guid? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetGuid(ordinal));

    /// <summary>
    /// Gets the short value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static short GetInt16(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, true, (reader, ordinal) => reader.GetInt16(ordinal));

    /// <summary>
    /// Gets the short value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static short GetInt16(this IDataReader instance, string columnName, short valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetInt16(ordinal));

    /// <summary>
    /// Gets the nullable short value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static short? GetNullableInt16(this IDataReader instance, string columnName, short? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetInt16(ordinal));

    /// <summary>
    /// Gets the int value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static int GetInt32(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, true, (reader, ordinal) => reader.GetInt32(ordinal));

    /// <summary>
    /// Gets the int value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static int GetInt32(this IDataReader instance, string columnName, int valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetInt32(ordinal));

    /// <summary>
    /// Gets the nullable int value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static int? GetNullableInt32(this IDataReader instance, string columnName, int? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetInt32(ordinal));

    /// <summary>
    /// Gets the long value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static long GetInt64(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, true, (reader, ordinal) => reader.GetInt64(ordinal));

    /// <summary>
    /// Gets the long value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static long GetInt64(this IDataReader instance, string columnName, long valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetInt64(ordinal));

    /// <summary>
    /// Gets the nullable long value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static long? GetNullableInt64(this IDataReader instance, string columnName, long? valueWhenDBNull = null, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetInt64(ordinal));

    /// <summary>
    /// Gets the (required) string value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    /// <exception cref="NullReferenceException"></exception>
    public static string GetString(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, string.Empty, true, true, (reader, ordinal) => reader.GetString(ordinal));

    /// <summary>
    /// Gets the (optional) string value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static string GetString(this IDataReader instance, string columnName, string valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetString(ordinal));

    /// <summary>
    /// Gets the (required) nullable string value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static string? GetNullableString(this IDataReader instance, string columnName, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, default, true, false, (reader, ordinal) => reader.GetString(ordinal));

    /// <summary>
    /// Gets the (optional) nullable string value.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="valueWhenDBNull">The value to use when the database value is DBNull.Value.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <exception cref="ArgumentOutOfRangeException">columnName</exception>
    public static string? GetNullableString(this IDataReader instance, string columnName, string? valueWhenDBNull, bool skipUnknownColumn = false)
        => instance.Invoke(columnName, skipUnknownColumn, valueWhenDBNull, true, false, (reader, ordinal) => reader.GetString(ordinal));

    /// <summary>
    /// Gets the byte array.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="skipUnknownColumn"></param>
    /// <param name="length">The length.</param>
    /// <returns>
    /// Empty array when no data, otherwise filled array with bytes.
    /// </returns>
    public static byte[] GetByteArray(this IDataReader instance, string columnName, bool skipUnknownColumn = false, int? length = null)
    {
        if (!instance.HasColumn(columnName))
        {
            if (skipUnknownColumn)
            {
                return Array.Empty<byte>();
            }
            throw new ArgumentOutOfRangeException(nameof(columnName), $"Column [{columnName}] could not be found");
        }

        var ordinal = instance.GetOrdinal(columnName);

        if (instance.IsDBNull(ordinal))
        {
            return Array.Empty<byte>();
        }

        if (length == null)
        {
            return (byte[])instance.GetValue(ordinal);
        }

        byte[] bytes = new byte[length.Value];
        instance.GetBytes(ordinal, 0, bytes, 0, length.Value);

        return bytes;
    }

    /// <summary>
    /// Maps the first entity of an IDataReader instance.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="mapFunction">The map function.</param>
    /// <returns>
    /// Mapped instance, or null when IDataReader.Read returns false.
    /// </returns>
    public static T? FindOne<T>(this IDataReader instance, Func<IDataReader, T> mapFunction)
        where T : class
        => instance.Read()
            ? mapFunction(instance)
            : default;

    /// <summary>
    /// Maps all entities of an IDataReader instance.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="mapFunction">The map function.</param>
    /// <returns>
    /// Zero or more mapped instances.
    /// </returns>
    public static IReadOnlyCollection<T> FindMany<T>(this IDataReader instance, Func<IDataReader, T> mapFunction)
    {
        var result = new List<T>();
        while (instance.Read())
        {
            result.Add(mapFunction(instance));
        }
        return result;
    }

    private static T Invoke<T>(this IDataReader instance, string columnName, bool skipUnknownColumn, T valueWhenDBNull, bool checkDbNull, bool throwOnDbNull, Func<IDataReader, int, T> mapFunction)
    {
        if (!instance.HasColumn(columnName))
        {
            if (skipUnknownColumn)
            {
                return valueWhenDBNull;
            }
            throw new ArgumentOutOfRangeException(nameof(columnName), $"Column [{columnName}] could not be found");
        }

        var ordinal = instance.GetOrdinal(columnName);

        if (checkDbNull && instance.IsDBNull(ordinal))
        {
            if (throwOnDbNull)
            {
                throw new DataException(string.Format("Column [{0}] is DBNull", columnName));
            }
            return valueWhenDBNull;
        }

        return mapFunction(instance, ordinal);
    }
}
