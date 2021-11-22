namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseCommandProvider
    {
        IDatabaseCommand Create(DatabaseOperation operation);
    }

    public interface IDatabaseCommandProvider<in TSource> : IDatabaseCommandProvider
    {
        IDatabaseCommand Create(TSource source, DatabaseOperation operation);
    }
}
