namespace CrossCutting.Common.Results;

public class ResultDictionaryBuilder
{
    private readonly Dictionary<string, Func<IReadOnlyDictionary<string, Result>, Result>> _resultset = new();

    public ResultDictionaryBuilder Add(string name, Func<Result> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, _ => value());
        return this;
    }

    public ResultDictionaryBuilder Add(string name, Func<IReadOnlyDictionary<string, Result>, Result> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

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

    public IReadOnlyDictionary<string, Result> Build()
    {
        var results = new Dictionary<string, Result>();

        foreach (var item in _resultset)
        {
            Result result = default!;

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                result = item.Value(results);
            }
            catch (Exception ex)
            {
                result = Result.Error(ex, $"Error occured while adding item with key {item.Key}, see Exception for details");
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

public class ResultDictionaryBuilder<T>
{
    private readonly Dictionary<string, Func<IReadOnlyDictionary<string, Result<T>>, Result<T>>> _resultset = new();

    public ResultDictionaryBuilder<T> Add(string name, Func<Result<T>> value)
    {
        _resultset.Add(name, _ => value());
        return this;
    }

    public ResultDictionaryBuilder<T> Add(string name, Func<IReadOnlyDictionary<string, Result<T>>, Result<T>> value)
    {
        _resultset.Add(name, value);
        return this;
    }

    public ResultDictionaryBuilder<T> Add(string name, Func<Result> value)
    {
        _resultset.Add(name, _ => Result.FromExistingResult<T>(value()));
        return this;
    }

    public ResultDictionaryBuilder<T> Add(string name, Func<IReadOnlyDictionary<string, Result<T>>, Result> value)
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

    public IReadOnlyDictionary<string, Result<T>> Build()
    {
        var results = new Dictionary<string, Result<T>>();

        Result<T> result = default!;
        foreach (var item in _resultset)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                result = item.Value(results);
            }
            catch (Exception ex)
            {
                result = Result.Error<T>(ex, $"Error occured while adding item with key {item.Key}, see Exception for details");
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
