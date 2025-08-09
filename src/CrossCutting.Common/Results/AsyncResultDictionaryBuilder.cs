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

    public AsyncResultDictionaryBuilder Add<T>(string name, T value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.Run<Result>(() => Result.Success(value)));
        return this;
    }

    public AsyncResultDictionaryBuilder Add<T>(string name, Func<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.Run<Result>(() => Result.Success(value())));
        return this;
    }

    public AsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Task<Result>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

        return this;
    }

    public AsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Task<Result<T>>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

        return this;
    }

    public AsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Func<Result>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

        return this;
    }

    public AsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Func<Result<T>>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

        return this;
    }

    public AsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Result> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            // For now, make it fail fast just like TakeWhileWithFirstNonMatching: stop on first error (but it still gets added to the results, so you can simply check for the first error)
            if (!item.IsSuccessful())
            {
                break;
            }
            counter++;
        }

        return this;
    }

    public AsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Result<T>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            // For now, make it fail fast just like TakeWhileWithFirstNonMatching: stop on first error (but it still gets added to the results, so you can simply check for the first error)
            if (!item.IsSuccessful())
            {
                break;
            }
            counter++;
        }

        return this;
    }

    public AsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

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

    public async Task<IReadOnlyDictionary<string, Func<Result>>> BuildLazy()
    {
        var results = new Dictionary<string, Func<Result>>();

        foreach (var item in _resultset)
        {
            Result result;
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                result = await item.Value.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                result = Result.Error(ex, "Exception occured");
            }
#pragma warning restore CA1031 // Do not catch general exception types
            results.Add(item.Key, () => result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }

    public async Task<IReadOnlyDictionary<string, Result>> Build()
    {
        var results = new Dictionary<string, Result>();

        foreach (var item in _resultset)
        {
            Result result;
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                result = (await item.Value.ConfigureAwait(false))
                    .EnsureNotNull($"Result with key {item.Key} returned a null result");
            }
            catch (Exception ex)
            {
                result = Result.Error(ex, "Exception occured");
            }
#pragma warning restore CA1031 // Do not catch general exception types
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

    public AsyncResultDictionaryBuilder<T> Add(string name, T value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.FromResult(Result.Success(value)));
        return this;
    }

    public AsyncResultDictionaryBuilder<T> Add(string name, Func<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, Task.Run(() => Result.Success(value())));
        return this;
    }

    public AsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Task<Result<T>>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

        return this;
    }

    public AsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Func<Result<T>>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

        return this;
    }

    public AsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Result<T>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            // For now, make it fail fast just like TakeWhileWithFirstNonMatching: stop on first error (but it still gets added to the results, so you can simply check for the first error)
            if (!item.IsSuccessful())
            {
                break;
            }
            counter++;
        }

        return this;
    }

    public AsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value)
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, item);
            counter++;
        }

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

    public async Task<IReadOnlyDictionary<string, Func<Result<T>>>> BuildLazy()
    {
        var results = new Dictionary<string, Func<Result<T>>>();

        foreach (var item in _resultset)
        {
            Result<T> result;
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                result = await item.Value.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                result = Result.Error<T>(ex, "Exception occured");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            results.Add(item.Key, () => result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }

    public async Task<IReadOnlyDictionary<string, Result<T>>> Build()
    {
        var results = new Dictionary<string, Result<T>>();

        foreach (var item in _resultset)
        {
            Result<T> result;
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                result = await item.Value.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                result = Result.Error<T>(ex, "Exception occured");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }
}
