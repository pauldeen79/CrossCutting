namespace CrossCutting.Common;

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
        => ValueCollectionBase.Equals(this, other, _equalityComparer);

    public override bool Equals(object? obj)
        => obj is { } && (ReferenceEquals(this, obj) || obj is ReadOnlyValueCollection<T> coll && Equals(coll));

    public override int GetHashCode()
        => unchecked(Items.Aggregate(0,
            (current, element) => (current * 397) ^ (element is null ? 0 : _equalityComparer.GetHashCode(element))
        ));

    public string ToString(string? format, IFormatProvider? formatProvider)
        => "[" + string.Join(", ", this.Select(e => ValueCollectionBase.FormatValue(e, formatProvider))) + "]";

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);
}
