namespace CrossCutting.Common.Results;

public class ResultDictionaryBuilder : IResultDictionaryBuilder
{
    private readonly Dictionary<string, Func<Result>> _resultset = new();
    private readonly IFuncDecorator _funcDecorator;

    public ResultDictionaryBuilder() : this(new LegacyFuncDecorator())
    {
    }

    public ResultDictionaryBuilder(IFuncDecorator funcDecorator)
    {
        ArgumentGuard.IsNotNull(funcDecorator, nameof(funcDecorator));

        _funcDecorator = funcDecorator;
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
            var result = _funcDecorator.Execute(item);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }

    private sealed class LegacyFuncDecorator : IFuncDecorator
    {
        public Result Execute(KeyValuePair<string, Func<Result>> taskItem)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return taskItem.Value()
                    .EnsureNotNull($"Result instance for item with key {taskItem.Key} is null");
            }
            catch (Exception ex)
            {
                return Result.Error(ex, $"Error occured while adding item with key {taskItem.Key}, see Exception for details");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}

public class ResultDictionaryBuilder<T> : IResultDictionaryBuilder<T>
{
    private readonly Dictionary<string, Func<Result<T>>> _resultset = new();
    private readonly IFuncDecorator<T> _funcDecorator;

    public ResultDictionaryBuilder() : this(new LegacyFuncDecorator())
    {
    }

    public ResultDictionaryBuilder(IFuncDecorator<T> funcDecorator)
    {
        ArgumentGuard.IsNotNull(funcDecorator, nameof(funcDecorator));

        _funcDecorator = funcDecorator;
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
            var result = _funcDecorator.Execute(item);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }

    private sealed class LegacyFuncDecorator : IFuncDecorator<T>
    {
        public Result<T> Execute(KeyValuePair<string, Func<Result<T>>> taskItem)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                return taskItem.Value()
                    .EnsureNotNull($"Result instance for item with key {taskItem.Key} is null");
            }
            catch (Exception ex)
            {
                return Result.Error<T>(ex, $"Error occured while adding item with key {taskItem.Key}, see Exception for details");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
