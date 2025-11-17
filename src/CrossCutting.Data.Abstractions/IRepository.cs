namespace CrossCutting.Data.Abstractions;

public interface IRepository<TEntity, in TIdentity>
    where TEntity : class
{
    Task<Result<TEntity>> AddAsync(TEntity instance, CancellationToken token);
    Task<Result<TEntity>> UpdateAsync(TEntity instance, CancellationToken token);
    Task<Result<TEntity>> DeleteAsync(TEntity instance, CancellationToken token);
    Task<Result<TEntity>> FindAsync(TIdentity identity, CancellationToken token);
    Task<Result<IReadOnlyCollection<TEntity>>> FindAllAsync(CancellationToken token);
    Task<Result<IPagedResult<TEntity>>> FindAllPagedAsync(int offset, int pageSize, CancellationToken token);
}
