namespace CrossCutting.Common.Results;

public class ResultDictionaryBuilder
{
    private readonly Dictionary<string, Func<Dictionary<string, Result>, Result>> _resultset = new();

    public ResultDictionaryBuilder Add(string name, Func<Result> value)
    {
        _resultset.Add(name, _ => value());
        return this;
    }

    public ResultDictionaryBuilder Add(string name, Func<Dictionary<string, Result>, Result> value)
    {
        _resultset.Add(name, value);
        return this;
    }

    public ResultDictionaryBuilder AddRange(string nameFormatString, Func<IEnumerable<Result>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value())
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, () => item);
            // For now, make it fail fast just like TakeWhileWithFirstNonMatching: stop on first error (but it still gets added to the results, so you can simply check for the first error)
            if (!item.IsSuccessful())
            {
                break;
            }
            counter++;
        }

        return this;
    }

    public Dictionary<string, Result> Build()
    {
        var results = new Dictionary<string, Result>();

        foreach (var item in _resultset)
        {
            var result = item.Value(results);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }
}

public class ResultDictionaryBuilder<T>
{
    private readonly Dictionary<string, Func<Dictionary<string, Result<T>>, Result<T>>> _resultset = new();

    public ResultDictionaryBuilder<T> Add(string name, Func<Result<T>> value)
    {
        _resultset.Add(name, _ => value());
        return this;
    }

    public ResultDictionaryBuilder<T> Add(string name, Func<Dictionary<string, Result<T>>, Result<T>> value)
    {
        _resultset.Add(name, value);
        return this;
    }

    public ResultDictionaryBuilder<T> Add(string name, Func<Result> value)
    {
        _resultset.Add(name, _ => Result.FromExistingResult<T>(value()));
        return this;
    }

    public ResultDictionaryBuilder<T> Add(string name, Func<Dictionary<string, Result<T>>, Result> value)
    {
        _resultset.Add(name, results => Result.FromExistingResult<T>(value(results)));
        return this;
    }

    public ResultDictionaryBuilder<T> AddRange(string nameFormatString, Func<IEnumerable<Result<T>>> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        var counter = 0;
        foreach (var item in value())
        {
            var name = string.Format(nameFormatString, counter);
            Add(name, () => item);
            // For now, make it fail fast just like TakeWhileWithFirstNonMatching: stop on first error (but it still gets added to the results, so you can simply check for the first error)
            if (!item.IsSuccessful())
            {
                break;
            }
            counter++;
        }

        return this;
    }

    public ResultDictionaryBuilder<T> AddRange(string nameFormatString, Func<IEnumerable<Result>> value)
    {
        return AddRange(nameFormatString, () => value().Select(x => Result.FromExistingResult<T>(x)));
    }

    public Dictionary<string, Result<T>> Build()
    {
        var results = new Dictionary<string, Result<T>>();

        foreach (var item in _resultset)
        {
            var result = item.Value(results);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }
}
