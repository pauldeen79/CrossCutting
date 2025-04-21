namespace CrossCutting.Common.Results;

public  class AsyncResultDictionaryBuilder
{
    private readonly Dictionary<string, Func<Task<Result>>> _resultset = new();

    public AsyncResultDictionaryBuilder Add<T>(string name, Func<Task<Result<T>>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, () => value().ContinueWith(x => (Result)x.Result, TaskScheduler.Current));
        return this;
    }

    public AsyncResultDictionaryBuilder Add(string name, Func<Task<Result>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public async Task<Dictionary<string, Result>> Build()
    {
        var results = new Dictionary<string, Result>();

        foreach (var item in _resultset)
        {
            var result = await item.Value().ConfigureAwait(false);
            results.Add(item.Key, result);
            // For now, make it fail fast just like TakeWhileWithFirstNonMatching: stop on first error (but it still gets added to the results, so you can simply check for the first error)
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }

    public async Task<Dictionary<string, Result>> BuildParallel()
    {
        var results = new Dictionary<string, Result>();

        var resultTasks = await Task.WhenAll(_resultset.Select(x => x.Value())).ConfigureAwait(false);
        var index = 0;

        foreach (var item in _resultset)
        {
            var result = resultTasks[index];
            index++;
            results.Add(item.Key, result);
            // For now, make it fail fast just like TakeWhileWithFirstNonMatching: stop on first error (but it still gets added to the results, so you can simply check for the first error)
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
    private readonly Dictionary<string, Func<Task<Result<T>>>> _resultset = new();

    public AsyncResultDictionaryBuilder<T> Add(string name, Func<Task<Result<T>>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public async Task<Dictionary<string, Result<T>>> Build()
    {
        var results = new Dictionary<string, Result<T>>();

        foreach (var item in _resultset)
        {
            var result = await item.Value().ConfigureAwait(false);
            results.Add(item.Key, result);
            // For now, make it fail fast just like TakeWhileWithFirstNonMatching: stop on first error (but it still gets added to the results, so you can simply check for the first error)
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }
    public async Task<Dictionary<string, Result<T>>> BuildParallel()
    {
        var results = new Dictionary<string, Result<T>>();

        var resultTasks = await Task.WhenAll(_resultset.Select(x => x.Value())).ConfigureAwait(false);
        var index = 0;

        foreach (var item in _resultset)
        {
            var result = resultTasks[index];
            index++;
            results.Add(item.Key, result);
            // For now, make it fail fast just like TakeWhileWithFirstNonMatching: stop on first error (but it still gets added to the results, so you can simply check for the first error)
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }
}
