namespace CrossCutting.Data.Abstractions;

public interface IRepository<TEntity, in TIdentity>
    where TEntity : class
{
    Task<Result<TEntity>> AddAsync(TEntity instance, CancellationToken cancellationToken);
    Task<Result<TEntity>> UpdateAsync(TEntity instance, CancellationToken cancellationToken);
    Task<Result<TEntity>> DeleteAsync(TEntity instance, CancellationToken cancellationToken);
    Task<Result<TEntity>> FindAsync(TIdentity identity, CancellationToken cancellationToken);
    Task<Result<IReadOnlyCollection<TEntity>>> FindAllAsync(CancellationToken cancellationToken);
    Task<Result<IPagedResult<TEntity>>> FindAllPagedAsync(int offset, int pageSize, CancellationToken cancellationToken);
}
