namespace CrossCutting.Data.Abstractions;

public interface IDatabaseCommandEntityProvider<T> : IDatabaseCommandEntityProvider<T, T>
{
}

public interface IDatabaseCommandEntityProvider<TEntity, TBuilder>
{
    public Func<TEntity, TBuilder>? CreateBuilderDelegate { get; }
    public Func<TBuilder, TEntity>? CreateEntityDelegate { get; }
    public Func<TBuilder, DatabaseOperation, TBuilder>? ResultEntityDelegate { get; }
    public Func<TBuilder, DatabaseOperation, IDataReader, TBuilder>? AfterReadDelegate { get; }
}
