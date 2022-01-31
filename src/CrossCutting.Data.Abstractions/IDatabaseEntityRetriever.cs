namespace CrossCutting.Data.Abstractions;

public interface IDatabaseEntityRetriever<out T> where T : class
{
    T? FindOne(IDatabaseCommand command);
    IReadOnlyCollection<T> FindMany(IDatabaseCommand command);
    IPagedResult<T> FindPaged(IPagedDatabaseCommand command);
}
