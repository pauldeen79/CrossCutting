namespace CrossCutting.Data.Abstractions;

public interface IDatabaseEntityRetrieverProvider
{
    Result<IDatabaseEntityRetriever<TResult>> Create<TResult>(object query) where TResult : class;
}
