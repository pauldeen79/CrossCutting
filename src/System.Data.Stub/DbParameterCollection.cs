namespace System.Data.Stub;

public sealed class DbParameterCollection : IDataParameterCollection
{
    private readonly IDictionary<string, object> dictionary = new Dictionary<string, object>();

    public object this[string parameterName] { get => dictionary[parameterName]; set => dictionary[parameterName] = value; }

    public object this[int index] { get => dictionary[dictionary.Keys.ElementAt(index)]; set => dictionary[dictionary.Keys.ElementAt(index)] = value; }

    public bool IsFixedSize => false;

    public bool IsReadOnly => false;

    public int Count => dictionary.Count;

    public bool IsSynchronized => true;

    public object SyncRoot => this;

    public int Add(object value)
    {
        dictionary.Add("_" + (dictionary.Count + 1), value);
        return dictionary.Count;
    }

    public void Clear() => dictionary.Clear();

    public bool Contains(string parameterName) => dictionary.ContainsKey(parameterName);

    public bool Contains(object value) => dictionary.Values.Contains(value);

    public void CopyTo(Array array, int index) => throw new NotImplementedException();

    public IEnumerator GetEnumerator() => dictionary.Select(kvp => (IDbDataParameter)kvp.Value).GetEnumerator();

    public int IndexOf(string parameterName) => dictionary.Keys.ToList().IndexOf(parameterName);

    public int IndexOf(object value) => dictionary.Values.ToList().IndexOf(value);

    public void Insert(int index, object value) => dictionary.Add("_" + index, value);

    public void Remove(object value)
        => dictionary.Where(kvp => kvp.Value.Equals(value))
            .Select(kvp => kvp.Key)
            .ToList()
            .ForEach(key => dictionary.Remove(key));

    public void RemoveAt(string parameterName) => dictionary.Remove(parameterName);

    public void RemoveAt(int index) => dictionary.Remove(dictionary.Keys.ElementAt(index));
}
