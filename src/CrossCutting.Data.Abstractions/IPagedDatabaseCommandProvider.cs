namespace CrossCutting.Data.Abstractions;

public interface IPagedDatabaseCommandProvider
{
    Result<IPagedDatabaseCommand> CreatePaged<TSource>(DatabaseOperation operation, int offset, int pageSize);
}

public interface IPagedDatabaseCommandProvider<in TSource>
{
    Result<IPagedDatabaseCommand> CreatePaged(TSource source, DatabaseOperation operation, int offset, int pageSize);
}
