namespace CrossCutting.Common.Results;

public class LazyResultDictionary : IDictionary<string, Result>
{
    private readonly Dictionary<string, Func<Result>> _resultset = new();

    public Result this[string key]
    {
        get
        {
            return _resultset[key]();
        }
        set
        {
            _resultset[key] = () => value;
        }
    }

    public ICollection<string> Keys => _resultset.Keys;

#pragma warning disable S2365 // Properties should not make collection or array copies
    public ICollection<Result> Values => _resultset.Values.Select(x => x()).ToArray();
#pragma warning restore S2365 // Properties should not make collection or array copies

    public int Count => _resultset.Count;

    public bool IsReadOnly => false;

    public LazyResultDictionary Add(string name, Func<Result> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public void Add(string key, Result value)
        => Add(key, () => value);

    public void Add(KeyValuePair<string, Result> item)
        => Add(item.Key, () => item.Value);

    public LazyResultDictionary AddRange(string nameFormatString, IEnumerable<Func<Result>> values)
    {
        values = ArgumentGuard.IsNotNull(values, nameof(values));

        var counter = 0;
        foreach (var item in values)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

        return this;
    }

    public void Clear() => _resultset.Clear();

    public bool Contains(KeyValuePair<string, Result> item) => _resultset.Contains(new KeyValuePair<string, Func<Result>>(item.Key, () => item.Value));

    public bool ContainsKey(string key) => _resultset.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, Result>[] array, int arrayIndex)
        => throw new NotSupportedException("You can't copy a lazy result dictionary");

    public IEnumerator<KeyValuePair<string, Result>> GetEnumerator() => _resultset.Select(x => new KeyValuePair<string, Result>(x.Key, x.Value())).GetEnumerator();

    public bool Remove(string key) => _resultset.Remove(key);

    public bool Remove(KeyValuePair<string, Result> item)
        => throw new NotSupportedException("You can't remove an item from a lazy result dictionary by value");

    public bool TryGetValue(string key, out Result value)
    {
        var result = _resultset.TryGetValue(key, out var dlg);
        if (dlg is not null)
        {
            value = dlg();
        }
        else
        {
            value = default;
        }

        return result;
    }

    IEnumerator IEnumerable.GetEnumerator() => _resultset.GetEnumerator();
}

public class LazyResultDictionary<T> : IDictionary<string, Result<T>>
{
    private readonly Dictionary<string, Func<Result<T>>> _resultset = new();

    public Result<T> this[string key]
    {
        get
        {
            return _resultset[key]();
        }
        set
        {
            _resultset[key] = () => value;
        }
    }

    public ICollection<string> Keys => _resultset.Keys;

#pragma warning disable S2365 // Properties should not make collection or array copies
    public ICollection<Result<T>> Values => _resultset.Values.Select(x => x()).ToArray();
#pragma warning restore S2365 // Properties should not make collection or array copies

    public int Count => _resultset.Count;

    public bool IsReadOnly => false;

    public LazyResultDictionary<T> Add(string name, Func<Result<T>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public void Add(string key, Result<T> value)
        => Add(key, () => value);

    public void Add(KeyValuePair<string, Result<T>> item)
        => Add(item.Key, () => item.Value);

    public LazyResultDictionary<T> AddRange(string nameFormatString, IEnumerable<Func<Result<T>>> values)
    {
        values = ArgumentGuard.IsNotNull(values, nameof(values));

        var counter = 0;
        foreach (var item in values)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

        return this;
    }

    public void Clear() => _resultset.Clear();

    public bool Contains(KeyValuePair<string, Result<T>> item) => _resultset.Contains(new KeyValuePair<string, Func<Result<T>>>(item.Key, () => item.Value));

    public bool ContainsKey(string key) => _resultset.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, Result<T>>[] array, int arrayIndex)
        => throw new NotSupportedException("You can't copy a lazy result dictionary");

    public IEnumerator<KeyValuePair<string, Result<T>>> GetEnumerator() => _resultset.Select(x => new KeyValuePair<string, Result<T>>(x.Key, x.Value())).GetEnumerator();

    public bool Remove(string key) => _resultset.Remove(key);

    public bool Remove(KeyValuePair<string, Result<T>> item)
        => throw new NotSupportedException("You can't remove an item from a lazy result dictionary by value");

    public bool TryGetValue(string key, out Result<T> value)
    {
        var result = _resultset.TryGetValue(key, out var dlg);
        if (dlg is not null)
        {
            value = dlg();
        }
        else
        {
            value = default;
        }

        return result;
    }

    IEnumerator IEnumerable.GetEnumerator() => _resultset.GetEnumerator();
}
