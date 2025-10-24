namespace CrossCutting.Common.Abstractions;

public interface IResultDictionaryBuilder
{
    IResultDictionaryBuilder Add(Func<Result> value);
    IResultDictionaryBuilder Add(string name, Func<Result> value);
    IResultDictionaryBuilder AddRange(string nameFormatString, Func<IEnumerable<Result>> value);
    IReadOnlyDictionary<string, Result> Build();
}

public interface IResultDictionaryBuilder<T>
{
    IResultDictionaryBuilder<T> Add(Func<Result<T>> value);
    IResultDictionaryBuilder<T> Add(Func<Result> value);
    IResultDictionaryBuilder<T> Add(string name, Func<Result<T>> value);
    IResultDictionaryBuilder<T> Add(string name, Func<Result> value);
    IResultDictionaryBuilder<T> AddRange(string nameFormatString, Func<IEnumerable<Result<T>>> value);
    IResultDictionaryBuilder<T> AddRange(string nameFormatString, Func<IEnumerable<Result>> value);
    IReadOnlyDictionary<string, Result<T>> Build();
}
