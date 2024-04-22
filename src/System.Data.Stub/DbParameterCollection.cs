namespace System.Data.Stub;

public sealed class DbParameterCollection : Common.DbParameterCollection
{
    private readonly IDictionary<string, object> dictionary = new Dictionary<string, object>();

    public override bool IsFixedSize => false;

    public override bool IsReadOnly => false;

    public override int Count => dictionary.Count;

    public override bool IsSynchronized => true;

    public override object SyncRoot => this;

    public override int Add(object value)
    {
        dictionary.Add("_" + (dictionary.Count + 1), value);
        return dictionary.Count;
    }

    public override void AddRange(Array values)
    {
        foreach (var value in values)
        {
            Add(value);
        }
    }

    public override void Clear() => dictionary.Clear();

    public override bool Contains(string value) => dictionary.Values.OfType<DbDataParameter>().Any(x => x.ParameterName == value);

    public override bool Contains(object value) => dictionary.Values.OfType<DbDataParameter>().Any(x => x.Value?.Equals(value) == true);

    public override void CopyTo(Array array, int index) => throw new NotImplementedException();

    public override IEnumerator GetEnumerator() => dictionary.Select(kvp => (IDbDataParameter)kvp.Value).GetEnumerator();

    public override int IndexOf(string parameterName) => dictionary.Keys.ToList().IndexOf(parameterName);

    public override int IndexOf(object value) => dictionary.Values.ToList().IndexOf(value);

    public override void Insert(int index, object value) => dictionary.Add("_" + index, value);

    public override void Remove(object value)
        => dictionary.Where(kvp => kvp.Value.Equals(value))
            .Select(kvp => kvp.Key)
            .ToList()
            .ForEach(key => dictionary.Remove(key));

    public override void RemoveAt(string parameterName) => dictionary.Remove(parameterName);

    public override void RemoveAt(int index) => dictionary.Remove(dictionary.Keys.ElementAt(index));

    protected override Common.DbParameter GetParameter(int index)
        => new DbDataParameter { ParameterName = dictionary.Keys.ElementAt(index), Value = dictionary[dictionary.Keys.ElementAt(index)] };

    protected override Common.DbParameter GetParameter(string parameterName)
        => new DbDataParameter { ParameterName = parameterName, Value = dictionary.Values.OfType<DbDataParameter>().First(x => x.ParameterName == parameterName) };

    protected override void SetParameter(int index, Common.DbParameter value)
        => dictionary[dictionary.Keys.ElementAt(index)] = value;

    protected override void SetParameter(string parameterName, Common.DbParameter value)
        => dictionary[parameterName] = value;
}
