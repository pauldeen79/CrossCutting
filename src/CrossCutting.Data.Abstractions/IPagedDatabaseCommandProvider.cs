namespace CrossCutting.Data.Abstractions
{
    public interface IPagedDatabaseCommandProvider : IDatabaseCommandProvider
    {
        IPagedDatabaseCommand CreatePaged(DatabaseOperation operation, int offset, int pageSize);
    }

    public interface IPagedDatabaseCommandProvider<in TSource> : IPagedDatabaseCommandProvider, IDatabaseCommandProvider<TSource>
    {
        IPagedDatabaseCommand CreatePaged(TSource source, DatabaseOperation operation, int offset, int pageSize);
    }
}
