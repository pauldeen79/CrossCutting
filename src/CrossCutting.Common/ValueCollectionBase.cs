namespace CrossCutting.Common;

internal static class ValueCollectionBase
{
    internal static string? FormatValue(object? value, IFormatProvider? formatProvider)
        => value switch
        {
            null => "∅",
            bool b => b ? "true" : "false",
            string s => $"\"{s}\"",
            char c => $"\'{c}\'",
            DateTime dt => dt.ToString("o", formatProvider),
            IFormattable @if => @if.ToString(null, formatProvider),
            IEnumerable ie => "[" + string.Join(", ", ie.Cast<object>().Select(e => FormatValue(e, formatProvider))) + "]",
            _ => value.ToString()
        };

    internal static bool Equals<T>(IEnumerable<T> instance, IEnumerable<T>? other, IEqualityComparer<T> equalityComparer)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(instance, other))
        {
            return true;
        }

        using var enumerator1 = instance.GetEnumerator();
        using var enumerator2 = other.GetEnumerator();

        while (enumerator1.MoveNext())
        {
            if (!enumerator2.MoveNext() || !(equalityComparer!).Equals(enumerator1.Current, enumerator2.Current))
            {
                return false;
            }
        }

        return !enumerator2.MoveNext(); //both enumerations reached the end
    }
}
