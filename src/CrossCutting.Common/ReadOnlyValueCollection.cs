﻿namespace CrossCutting.Common;

public sealed class ReadOnlyValueCollection<T> : ReadOnlyCollection<T>, IEquatable<ReadOnlyValueCollection<T>>, IFormattable
{
    private readonly IEqualityComparer<T> _equalityComparer;

    public ReadOnlyValueCollection() : this(Enumerable.Empty<T>())
    {
    }

    public ReadOnlyValueCollection(IEqualityComparer<T>? equalityComparer) : this(Enumerable.Empty<T>(), equalityComparer)
    {
    }

    public ReadOnlyValueCollection(IEnumerable<T> list, IEqualityComparer<T>? equalityComparer = null) : base(list.ToList())
        => _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

    public bool Equals(ReadOnlyValueCollection<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        using var enumerator1 = GetEnumerator();
        using var enumerator2 = other.GetEnumerator();

        while (enumerator1.MoveNext())
        {
            if (!enumerator2.MoveNext() || !(_equalityComparer!).Equals(enumerator1.Current, enumerator2.Current))
            {
                return false;
            }
        }

        return !enumerator2.MoveNext(); //both enumerations reached the end
    }

    public override bool Equals(object? obj)
        => obj is { } && (ReferenceEquals(this, obj) || obj is ReadOnlyValueCollection<T> coll && Equals(coll));

    public override int GetHashCode()
        => unchecked(Items.Aggregate(0,
            (current, element) => (current * 397) ^ (element is null ? 0 : _equalityComparer.GetHashCode(element))
        ));

    public string ToString(string? format, IFormatProvider? formatProvider)
        => "[" + string.Join(", ", this.Select(e => FormatValue(e, formatProvider))) + "]";

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    private static string? FormatValue(object? value, IFormatProvider? formatProvider)
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
}