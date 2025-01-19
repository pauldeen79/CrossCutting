namespace CrossCutting.Common.Abstractions;

public interface IResultDictionaryContainer
{
    Dictionary<string, Result> Results { get; }
}

public interface IResultDictionaryContainer<T>
{
    Dictionary<string, Result<T>> Results { get; }
}
