namespace CrossCutting.Data.Abstractions;

public interface IRepository<TEntity, in TIdentity>
    where TEntity : class
{
    Result<TEntity> Add(TEntity instance);
    Task<Result<TEntity>> AddAsync(TEntity instance, CancellationToken cancellationToken);
    Result<TEntity> Update(TEntity instance);
    Task<Result<TEntity>> UpdateAsync(TEntity instance, CancellationToken cancellationToken);
    Result<TEntity> Delete(TEntity instance);
    Task<Result<TEntity>> DeleteAsync(TEntity instance, CancellationToken cancellationToken);
    Result<TEntity> Find(TIdentity identity);
    Task<Result<TEntity>> FindAsync(TIdentity identity, CancellationToken cancellationToken);
    Result<IReadOnlyCollection<TEntity>> FindAll();
    Task<Result<IReadOnlyCollection<TEntity>>> FindAllAsync(CancellationToken cancellationToken);
    Result<IPagedResult<TEntity>> FindAllPaged(int offset, int pageSize);
    Task<Result<IPagedResult<TEntity>>> FindAllPagedAsync(int offset, int pageSize, CancellationToken cancellationToken);
}
