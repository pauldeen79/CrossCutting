namespace CrossCutting.Data.Abstractions;

public interface IDatabaseCommandEntityProvider<T> : IDatabaseCommandEntityProvider<T, T>
{
}

public interface IDatabaseCommandEntityProvider<TEntity, TBuilder>
{
    public OnCreateBuilder<TEntity, TBuilder>? OnCreateBuilder { get; }
    public OnCreateEntity<TBuilder, TEntity>? OnCreateEntity { get; }
    public OnResultEntityCreated<TBuilder>? OnCreateResultEntity { get; }
    public OnReadCompleted<TBuilder>? OnReadComplete { get; }
}

public delegate TBuilder OnCreateBuilder<in TEntity, out TBuilder>(TEntity entity);
public delegate TEntity OnCreateEntity<in TBuilder, out TEntity>(TBuilder builder);
public delegate TBuilder OnResultEntityCreated<TBuilder>(TBuilder resultEntity, DatabaseOperation operation);
public delegate TBuilder OnReadCompleted<TBuilder>(TBuilder resultEntity, DatabaseOperation operation, IDataReader reader);
