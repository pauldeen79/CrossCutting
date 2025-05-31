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

    public DeferredResultDictionaryBuilder Add(string name, Result value)
        => Add(name, () => value);

    public DeferredResultDictionaryBuilder Add(string name, object? value)
        => Add(name, () => Result.Success(value));

    public DeferredResultDictionaryBuilder Add(string name, Func<object?> value)
        => Add(name, () => Result.Success(value()));

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

    public DeferredResultDictionaryBuilder<T> Add(string name, Result<T> value)
        => Add(name, () => value);

    public DeferredResultDictionaryBuilder<T> Add(string name, T value)
        => Add(name, () => Result.Success(value));

    public DeferredResultDictionaryBuilder<T> Add(string name, Func<T> value)
        => Add(name, () => Result.Success(value()));

    public IReadOnlyDictionary<string, Func<Result<T>>> Build()
        => _resultset;
}
