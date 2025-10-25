namespace CrossCutting.Common.Abstractions;

public interface IAsyncResultDictionaryBuilder
{
    IAsyncResultDictionaryBuilder Add(Func<Result> value);
    IAsyncResultDictionaryBuilder Add(Result value);
    IAsyncResultDictionaryBuilder Add(string name, Func<Result> value);
    IAsyncResultDictionaryBuilder Add(string name, Result value);
    IAsyncResultDictionaryBuilder Add(string name, Func<Task<Result>> value);
    IAsyncResultDictionaryBuilder Add(Func<Task<Result>> value);
    IAsyncResultDictionaryBuilder Add<T>(Func<Result<T>> value);
    IAsyncResultDictionaryBuilder Add<T>(Func<T> value);
    IAsyncResultDictionaryBuilder Add<T>(Result<T> value);
    IAsyncResultDictionaryBuilder Add<T>(string name, Func<Result<T>> value);
    IAsyncResultDictionaryBuilder Add<T>(string name, Func<T> value);
    IAsyncResultDictionaryBuilder Add<T>(string name, Result<T> value);
    IAsyncResultDictionaryBuilder Add<T>(string name, T value);
    IAsyncResultDictionaryBuilder Add<T>(T value);
    IAsyncResultDictionaryBuilder Add<T>(Func<Task<Result<T>>> value);
    IAsyncResultDictionaryBuilder Add<T>(string name, Func<Task<Result<T>>> value);
    IAsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Func<Result>> value);
    IAsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Result> value);
    IAsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Func<Task<Result>>> value);
    IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Func<Task<Result<T>>>> value);
    IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Func<Result<T>>> value);
    IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Result<T>> value);
    IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<T> value);
    Task<IReadOnlyDictionary<string, Result>> Build();
    IReadOnlyDictionary<string, Func<Task<Result>>> BuildDeferred();
}

public interface IAsyncResultDictionaryBuilder<T>
{
    IAsyncResultDictionaryBuilder<T> Add(string name, Func<Result<T>> value);
    IAsyncResultDictionaryBuilder<T> Add(string name, Func<T> value);
    IAsyncResultDictionaryBuilder<T> Add(string name, Result<T> value);
    IAsyncResultDictionaryBuilder<T> Add(string name, T value);
    IAsyncResultDictionaryBuilder<T> Add(string name, Func<Task<Result<T>>> value);
    IAsyncResultDictionaryBuilder<T> Add(Func<Task<Result<T>>> value);
    IAsyncResultDictionaryBuilder<T> Add(Func<Result<T>> value);
    IAsyncResultDictionaryBuilder<T> Add(Result<T> value);
    IAsyncResultDictionaryBuilder<T> Add(T value);
    IAsyncResultDictionaryBuilder<T> Add(Func<T> value);
    IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Func<Result<T>>> value);
    IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Result<T>> value);
    IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<T> value);
    IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Func<Task<Result<T>>>> value);
    Task<IReadOnlyDictionary<string, Result<T>>> Build();
    IReadOnlyDictionary<string, Func<Task<Result<T>>>> BuildDeferred();
}
