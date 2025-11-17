namespace CrossCutting.Common.Results;

public class ResultDictionaryBuilder : IResultDictionaryBuilder
{
    private readonly Dictionary<string, Func<Result>> _resultset = new();
    private readonly List<IFuncInterceptor> _interceptors;

    public ResultDictionaryBuilder() : this([new LegacyFuncInterceptor()])
    {
    }

    public ResultDictionaryBuilder(IEnumerable<IFuncInterceptor> interceptors)
    {
        ArgumentGuard.IsNotNull(interceptors, nameof(interceptors));

        _interceptors = interceptors.OrderBy(x => (x as IOrderContainer)?.Order).ToList();
    }

    public IResultDictionaryBuilder Add(Func<Result> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IResultDictionaryBuilder Add(string name, Func<Result> value)
    {
        value = ArgumentGuard.IsNotNull(value, nameof(value));

        _resultset.Add(name, value);
        return this;
    }

    public IResultDictionaryBuilder AddRange(string nameFormatString, Func<IEnumerable<Result>> value)
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
            var result = DoExecute(item);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }

    private Result DoExecute(KeyValuePair<string, Func<Result>> item)
    {
        var index = 0;

        Result Next()
        {
            if (index < _interceptors.Count)
            {
                return _interceptors[index++].Execute(item, Next);
            }

            return item.Value.Invoke();
        }

        return Next();
    }

    private sealed class LegacyFuncInterceptor : IFuncInterceptor
    {
        public Result Execute(KeyValuePair<string, Func<Result>> item, Func<Result> next)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return next()
                    .EnsureNotNull($"Result instance for item with key {item.Key} is null");
            }
            catch (Exception ex)
            {
                return Result.Error(ex, $"Error occured while adding item with key {item.Key}, see Exception for details");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}

public class ResultDictionaryBuilder<T> : IResultDictionaryBuilder<T>
{
    private readonly Dictionary<string, Func<Result<T>>> _resultset = new();
    private readonly List<IFuncInterceptor<T>> _interceptors;

    public ResultDictionaryBuilder() : this([new LegacyFuncInterceptor()])
    {
    }

    public ResultDictionaryBuilder(IEnumerable<IFuncInterceptor<T>> interceptors)
    {
        ArgumentGuard.IsNotNull(interceptors, nameof(interceptors));

        _interceptors = interceptors.OrderBy(x => (x as IOrderContainer)?.Order).ToList();
    }

    public IResultDictionaryBuilder<T> Add(Func<Result<T>> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IResultDictionaryBuilder<T> Add(string name, Func<Result<T>> value)
    {
        _resultset.Add(name, value);
        return this;
    }

    public IResultDictionaryBuilder<T> Add(Func<Result> value)
        => Add((_resultset.Count + 1).ToString("D4"), value);

    public IResultDictionaryBuilder<T> Add(string name, Func<Result> value)
    {
        _resultset.Add(name, () => Result.FromExistingResult<T>(value()));
        return this;
    }

    public IResultDictionaryBuilder<T> AddRange(string nameFormatString, Func<IEnumerable<Result<T>>> value)
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

    public IResultDictionaryBuilder<T> AddRange(string nameFormatString, Func<IEnumerable<Result>> value)
        => AddRange(nameFormatString, () => value().Select(x => Result.FromExistingResult<T>(x)));

    public IReadOnlyDictionary<string, Result<T>> Build()
    {
        var results = new Dictionary<string, Result<T>>();

        foreach (var item in _resultset)
        {
            var result = DoExecute(item);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }

    private Result<T> DoExecute(KeyValuePair<string, Func<Result<T>>> item)
    {
        var index = 0;

        Result<T> Next()
        {
            if (index < _interceptors.Count)
            {
                return _interceptors[index++].Execute(item, Next);
            }

            return item.Value.Invoke();
        }

        return Next();
    }

    private sealed class LegacyFuncInterceptor : IFuncInterceptor<T>
    {
        public Result<T> Execute(KeyValuePair<string, Func<Result<T>>> item, Func<Result<T>> next)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return next()
                    .EnsureNotNull($"Result instance for item with key {item.Key} is null");
            }
            catch (Exception ex)
            {
                return Result.Error<T>(ex, $"Error occured while adding item with key {item.Key}, see Exception for details");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
