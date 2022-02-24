namespace CrossCutting.Data.Abstractions;

public interface IDatabaseCommandProvider
{
    IDatabaseCommand Create<TSource>(DatabaseOperation operation);
}

public interface IDatabaseCommandProvider<in TSource>
{
    IDatabaseCommand Create(TSource source, DatabaseOperation operation);
}
