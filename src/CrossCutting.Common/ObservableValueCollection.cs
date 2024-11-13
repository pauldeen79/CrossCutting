namespace CrossCutting.Common;

public sealed class ObservableValueCollection<T>(IEnumerable<T> list, IEqualityComparer<T>? equalityComparer = null) : ObservableCollection<T>(list.ToList()), IEquatable<ObservableCollection<T>>, IFormattable
{
    private readonly IEqualityComparer<T> _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

    public ObservableValueCollection() : this(Enumerable.Empty<T>())
    {
    }

    public ObservableValueCollection(IEqualityComparer<T>? equalityComparer) : this(Enumerable.Empty<T>(), equalityComparer)
    {
    }

    public bool Equals(ObservableCollection<T>? other)
        => ValueCollectionBase.Equals(this, other, _equalityComparer);

    public override bool Equals(object? obj)
        => obj is { } && (ReferenceEquals(this, obj) || obj is ObservableValueCollection<T> coll && Equals(coll));

    public override int GetHashCode()
        => unchecked(Items.Aggregate(0,
            (current, element) => (current * 397) ^ (element is null ? 0 : _equalityComparer.GetHashCode(element))
        ));

    public string ToString(string? format, IFormatProvider? formatProvider)
        => "[" + string.Join(", ", this.Select(e => ValueCollectionBase.FormatValue(e, formatProvider))) + "]";

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);
}
