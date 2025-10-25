namespace CrossCutting.Common.Results;

public class AsyncResultDictionaryBuilder : IAsyncResultDictionaryBuilder
{
    private readonly Dictionary<string, Func<Task<Result>>> _resultset = new();
    private readonly ITaskDecorator _taskDecorator;

    public AsyncResultDictionaryBuilder() : this(new LegacyTaskDecorator())
    {
    }

    public AsyncResultDictionaryBuilder(ITaskDecorator taskDecorator)
    {
        ArgumentGuard.IsNotNull(taskDecorator, nameof(taskDecorator));

        _taskDecorator = taskDecorator;
    }

    public IAsyncResultDictionaryBuilder Add<T>(Func<Task<Result<T>>> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder Add<T>(string name, Func<Task<Result<T>>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => value().ContinueWith(x => (Result)x.Result, TaskScheduler.Current));
        return this;
    }

    public IAsyncResultDictionaryBuilder Add(Func<Task<Result>> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder Add(string name, Func<Task<Result>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public IAsyncResultDictionaryBuilder Add<T>(Func<Result<T>> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder Add<T>(string name, Func<Result<T>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => Task.Run<Result>(() => value()));
        return this;
    }

    public IAsyncResultDictionaryBuilder Add(Func<Result> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder Add(string name, Func<Result> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => Task.Run(() => value()));
        return this;
    }

    public IAsyncResultDictionaryBuilder Add<T>(Result<T> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder Add<T>(string name, Result<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => Task.Run<Result>(() => value));
        return this;
    }

    public IAsyncResultDictionaryBuilder Add(Result value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder Add(string name, Result value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => Task.Run(() => value));
        return this;
    }

    public IAsyncResultDictionaryBuilder Add<T>(T value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder Add<T>(string name, T value)
    {
        _resultset.Add(name, () => Task.Run<Result>(() => Result.Success(value)));
        return this;
    }

    public IAsyncResultDictionaryBuilder Add<T>(Func<T> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder Add<T>(string name, Func<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => Task.Run<Result>(() => Result.Success(value())));
        return this;
    }

    public IAsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Func<Task<Result>>> value)
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

    public IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Func<Task<Result<T>>>> value)
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

    public IAsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Func<Result>> value)
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

    public IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Func<Result<T>>> value)
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

    public IAsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Result> value)
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

    public IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Result<T>> value)
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

    public IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<T> value)
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

    public IReadOnlyDictionary<string, Func<Task<Result>>> BuildDeferred()
    {
        var results = new Dictionary<string, Func<Task<Result>>>();

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
            var result = await _taskDecorator.Execute(item).ConfigureAwait(false);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }

    private sealed class LegacyTaskDecorator : ITaskDecorator
    {
        public async Task<Result> Execute(KeyValuePair<string, Func<Task<Result>>> taskDelegateItem)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return (await taskDelegateItem.Value().ConfigureAwait(false))
                    .EnsureNotNull($"Result with key {taskDelegateItem.Key} returned a null result");
            }
            catch (Exception ex)
            {
                return Result.Error(ex, "Exception occured");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}

public class AsyncResultDictionaryBuilder<T> : IAsyncResultDictionaryBuilder<T>
{
    private readonly Dictionary<string, Func<Task<Result<T>>>> _resultset = new();
    private readonly ITaskDecorator<T> _taskDecorator;

    public AsyncResultDictionaryBuilder() : this(new LegacyTaskDecorator())
    {
    }

    public AsyncResultDictionaryBuilder(ITaskDecorator<T> taskDecorator)
    {
        ArgumentGuard.IsNotNull(taskDecorator, nameof(taskDecorator));

        _taskDecorator = taskDecorator;
    }

    public IAsyncResultDictionaryBuilder<T> Add(Func<Task<Result<T>>> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder<T> Add(string name, Func<Task<Result<T>>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public IAsyncResultDictionaryBuilder<T> Add(Func<Result<T>> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder<T> Add(string name, Func<Result<T>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => Task.Run(value));
        return this;
    }

    public IAsyncResultDictionaryBuilder<T> Add(Result<T> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder<T> Add(string name, Result<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => Task.FromResult(value));
        return this;
    }

    public IAsyncResultDictionaryBuilder<T> Add(T value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder<T> Add(string name, T value)
    {
        _resultset.Add(name, () => Task.FromResult(Result.Success(value)));
        return this;
    }

    public IAsyncResultDictionaryBuilder<T> Add(Func<T> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IAsyncResultDictionaryBuilder<T> Add(string name, Func<T> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => Task.Run(() => Result.Success(value())));
        return this;
    }

    public IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Func<Task<Result<T>>>> value)
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

    public IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Func<Result<T>>> value)
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

    public IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Result<T>> value)
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

    public IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<T> value)
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

    public IReadOnlyDictionary<string, Func<Task<Result<T>>>> BuildDeferred()
    {
        var results = new Dictionary<string, Func<Task<Result<T>>>>();

        foreach (var item in _resultset)
        {
            results.Add(item.Key, item.Value);
        }

        return results;
    }

    public async Task<IReadOnlyDictionary<string, Result<T>>> Build()
    {
        var results = new Dictionary<string, Result<T>>();

        foreach (var item in _resultset)
        {
            var result = await _taskDecorator.Execute(item).ConfigureAwait(false);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }

    private sealed class LegacyTaskDecorator : ITaskDecorator<T>
    {
        public async Task<Result<T>> Execute(KeyValuePair<string, Func<Task<Result<T>>>> taskDelegateItem)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return (await taskDelegateItem.Value().ConfigureAwait(false))
                    .EnsureNotNull($"Result with key {taskDelegateItem.Key} returned a null result");
            }
            catch (Exception ex)
            {
                return Result.Error<T>(ex, "Exception occured");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
