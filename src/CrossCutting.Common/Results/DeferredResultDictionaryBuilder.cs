namespace CrossCutting.Common.Results;

public class DeferredResultDictionaryBuilder
{
    private readonly Dictionary<string, Func<Result>> _resultset = new();

    public DeferredResultDictionaryBuilder Add(string name, Func<Result> value)
    {
        ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public IReadOnlyDictionary<string, Func<Result>> Build()
        => _resultset;
}

public class DeferredResultDictionaryBuilder<T>
{
    private readonly Dictionary<string, Func<Result<T>>> _resultset = new();

    public DeferredResultDictionaryBuilder<T> Add(string name, Func<Result<T>> value)
    {
        ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public IReadOnlyDictionary<string, Func<Result<T>>> Build()
        => _resultset;
}
