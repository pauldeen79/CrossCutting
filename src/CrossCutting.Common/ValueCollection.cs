﻿namespace CrossCutting.Common;

public sealed class ValueCollection<T> : Collection<T>, IEquatable<ValueCollection<T>>, IFormattable
{
    private readonly IEqualityComparer<T> _equalityComparer;

    public ValueCollection() : this(Enumerable.Empty<T>())
    {
    }

    public ValueCollection(IEqualityComparer<T>? equalityComparer) : this(Enumerable.Empty<T>(), equalityComparer)
    {
    }

    public ValueCollection(IEnumerable<T> list, IEqualityComparer<T>? equalityComparer = null) : base(list.ToList())
        => _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

    public bool Equals(ValueCollection<T>? other)
        => ValueCollectionBase.Equals(this, other, _equalityComparer);

    public override bool Equals(object? obj)
        => obj is { } && (ReferenceEquals(this, obj) || obj is ValueCollection<T> coll && Equals(coll));

    public override int GetHashCode()
        => unchecked(Items.Aggregate(0,
            (current, element) => (current * 397) ^ (element is null ? 0 : _equalityComparer.GetHashCode(element))
        ));

    public string ToString(string? format, IFormatProvider? formatProvider)
        => "[" + string.Join(", ", this.Select(e => ValueCollectionBase.FormatValue(e, formatProvider))) + "]";

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);
}
