namespace CrossCutting.Common.Results;

public class AsyncResultDictionaryBuilder
{
    private readonly Dictionary<string, Task<Result>> _resultset = new();

    public AsyncResultDictionaryBuilder Add<T>(string name, Task<Result<T>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value.ContinueWith(x => (Result)x.Result, TaskScheduler.Current));
        return this;
    }

    public AsyncResultDictionaryBuilder Add(string name, Task<Result> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public AsyncResultDictionaryBuilder Add<T>(string name, Func<Result<T>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.Run<Result>(() => value()));
        return this;
    }

    public AsyncResultDictionaryBuilder Add(string name, Func<Result> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.Run(() => value()));
        return this;
    }

    public AsyncResultDictionaryBuilder Add<T>(string name, Result<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.Run<Result>(() => value));
        return this;
    }

    public AsyncResultDictionaryBuilder Add(string name, Result value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.Run(() => value));
        return this;
    }

    public IReadOnlyDictionary<string, Task<Result>> BuildDeferred()
    {
        var results = new Dictionary<string, Task<Result>>();

        foreach (var item in _resultset)
        {
            results.Add(item.Key, item.Value);
        }

        return results;
    }

    public async Task<IReadOnlyDictionary<string, Result>> Build()
    {
        var results = new Dictionary<string, Result>();

        foreach (var item in _resultset)
        {
            var result = await item.Value.ConfigureAwait(false);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }
}

public class AsyncResultDictionaryBuilder<T>
{
    private readonly Dictionary<string, Task<Result<T>>> _resultset = new();

    public AsyncResultDictionaryBuilder<T> Add(string name, Task<Result<T>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public AsyncResultDictionaryBuilder<T> Add(string name, Func<Result<T>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.Run(value));
        return this;
    }

    public AsyncResultDictionaryBuilder<T> Add(string name, Result<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.FromResult(value));
        return this;
    }

    public IReadOnlyDictionary<string, Task<Result<T>>> BuildDeferred()
    {
        var results = new Dictionary<string, Task<Result<T>>>();

        foreach (var item in _resultset)
        {
            results.Add(item.Key, item.Value);
        }

        return results;
    }

    public async Task<IReadOnlyDictionary<string, Func<Result<T>>>> Build()
    {
        var results = new Dictionary<string, Func<Result<T>>>();

        foreach (var item in _resultset)
        {
            var result = await item.Value.ConfigureAwait(false);
            results.Add(item.Key, () => result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }
}
