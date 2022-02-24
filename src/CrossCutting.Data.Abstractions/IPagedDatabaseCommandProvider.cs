namespace CrossCutting.Data.Abstractions;

public interface IPagedDatabaseCommandProvider
{
    IPagedDatabaseCommand CreatePaged<TSource>(DatabaseOperation operation, int offset, int pageSize);
}

public interface IPagedDatabaseCommandProvider<in TSource>
{
    IPagedDatabaseCommand CreatePaged(TSource source, DatabaseOperation operation, int offset, int pageSize);
}
