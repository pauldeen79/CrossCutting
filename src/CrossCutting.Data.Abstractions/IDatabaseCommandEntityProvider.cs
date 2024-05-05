namespace CrossCutting.Data.Abstractions;

public interface IDatabaseCommandEntityProvider<T> : IDatabaseCommandEntityProvider<T, T>
{
}

public interface IDatabaseCommandEntityProvider<TEntity, TBuilder>
{
    CreateBuilderHandler<TEntity, TBuilder>? CreateBuilder { get; }
    CreateEntityHandler<TBuilder, TEntity>? CreateEntity { get; }
    CreateResultEntityHandler<TBuilder>? CreateResultEntity { get; }
    AfterReadHandler<TBuilder>? AfterRead { get; }
}

public delegate TBuilder CreateBuilderHandler<in TEntity, out TBuilder>(TEntity entity);
public delegate TEntity CreateEntityHandler<in TBuilder, out TEntity>(TBuilder builder);
public delegate TBuilder CreateResultEntityHandler<TBuilder>(TBuilder builder, DatabaseOperation operation);
public delegate TBuilder AfterReadHandler<TBuilder>(TBuilder builder, DatabaseOperation operation, IDataReader reader);
public delegate IDatabaseCommand CreateCommandHandler<in TBuilder>(TBuilder builder, DatabaseOperation operation);
