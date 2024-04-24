namespace CrossCutting.Data.Abstractions;

public interface IRepository<TEntity, in TIdentity>
    where TEntity : class
{
    TEntity Add(TEntity instance);
    Task<TEntity> AddAsync(TEntity instance, CancellationToken cancellationToken);
    TEntity Update(TEntity instance);
    Task<TEntity> UpdateAsync(TEntity instance, CancellationToken cancellationToken);
    TEntity Delete(TEntity instance);
    Task<TEntity> DeleteAsync(TEntity instance, CancellationToken cancellationToken);
    TEntity? Find(TIdentity identity);
    Task<TEntity?> FindAsync(TIdentity identity, CancellationToken cancellationToken);
    IReadOnlyCollection<TEntity> FindAll();
    Task<IReadOnlyCollection<TEntity>> FindAllAsync(CancellationToken cancellationToken);
    IPagedResult<TEntity> FindAllPaged(int offset, int pageSize);
    Task<IPagedResult<TEntity>> FindAllPagedAsync(int offset, int pageSize, CancellationToken cancellationToken);
}
