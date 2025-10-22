namespace CrossCutting.Common.Abstractions;

public interface IAsyncResultDictionaryBuilder
{
    IAsyncResultDictionaryBuilder Add(Func<Result> value);
    IAsyncResultDictionaryBuilder Add(Result value);
    IAsyncResultDictionaryBuilder Add(string name, Func<Result> value);
    IAsyncResultDictionaryBuilder Add(string name, Result value);
    IAsyncResultDictionaryBuilder Add(string name, Task<Result> value);
    IAsyncResultDictionaryBuilder Add(Task<Result> value);
    IAsyncResultDictionaryBuilder Add<T>(Func<Result<T>> value);
    IAsyncResultDictionaryBuilder Add<T>(Func<T> value);
    IAsyncResultDictionaryBuilder Add<T>(Result<T> value);
    IAsyncResultDictionaryBuilder Add<T>(string name, Func<Result<T>> value);
    IAsyncResultDictionaryBuilder Add<T>(string name, Func<T> value);
    IAsyncResultDictionaryBuilder Add<T>(string name, Result<T> value);
    IAsyncResultDictionaryBuilder Add<T>(string name, T value);
    IAsyncResultDictionaryBuilder Add<T>(string name, Task<Result<T>> value);
    IAsyncResultDictionaryBuilder Add<T>(T value);
    IAsyncResultDictionaryBuilder Add<T>(Task<Result<T>> value);
    IAsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Func<Result>> value);
    IAsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Result> value);
    IAsyncResultDictionaryBuilder AddRange(string nameFormatString, IEnumerable<Task<Result>> value);
    IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Func<Result<T>>> value);
    IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Result<T>> value);
    IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<T> value);
    IAsyncResultDictionaryBuilder AddRange<T>(string nameFormatString, IEnumerable<Task<Result<T>>> value);
    Task<IReadOnlyDictionary<string, Result>> Build();
    IReadOnlyDictionary<string, Task<Result>> BuildDeferred();
    Task<IReadOnlyDictionary<string, Func<Result>>> BuildLazy();
}

public interface IAsyncResultDictionaryBuilder<T>
{
    IAsyncResultDictionaryBuilder<T> Add(string name, Func<Result<T>> value);
    IAsyncResultDictionaryBuilder<T> Add(string name, Func<T> value);
    IAsyncResultDictionaryBuilder<T> Add(string name, Result<T> value);
    IAsyncResultDictionaryBuilder<T> Add(string name, T value);
    IAsyncResultDictionaryBuilder<T> Add(string name, Task<Result<T>> value);
    IAsyncResultDictionaryBuilder<T> Add(Task<Result<T>> value);
    IAsyncResultDictionaryBuilder<T> Add(Func<Result<T>> value);
    IAsyncResultDictionaryBuilder<T> Add(Result<T> value);
    IAsyncResultDictionaryBuilder<T> Add(T value);
    IAsyncResultDictionaryBuilder<T> Add(Func<T> value);
    IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Func<Result<T>>> value);
    IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Result<T>> value);
    IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<T> value);
    IAsyncResultDictionaryBuilder<T> AddRange(string nameFormatString, IEnumerable<Task<Result<T>>> value);
    Task<IReadOnlyDictionary<string, Result<T>>> Build();
    IReadOnlyDictionary<string, Task<Result<T>>> BuildDeferred();
    Task<IReadOnlyDictionary<string, Func<Result<T>>>> BuildLazy();
}
