namespace CrossCutting.Data.Abstractions.Extensions;

public static class RepositoryExtensions
{
    public static Task<TEntity> AddAsync<TEntity, TIdentity>(this IRepository<TEntity, TIdentity> repository, TEntity instance)
        where TEntity : class
        => repository.AddAsync(instance, CancellationToken.None);

    public static Task<TEntity> UpdateAsync<TEntity, TIdentity>(this IRepository<TEntity, TIdentity> repository, TEntity instance)
        where TEntity : class
        => repository.UpdateAsync(instance, CancellationToken.None);

    public static Task<TEntity> DeleteAsync<TEntity, TIdentity>(this IRepository<TEntity, TIdentity> repository, TEntity instance)
        where TEntity : class
        => repository.DeleteAsync(instance, CancellationToken.None);

    public static Task<TEntity?> FindAsync<TEntity, TIdentity>(this IRepository<TEntity, TIdentity> repository, TIdentity identity)
        where TEntity : class
        => repository.FindAsync(identity, CancellationToken.None);

    public static Task<IReadOnlyCollection<TEntity>> FindAllAsync<TEntity, TIdentity>(this IRepository<TEntity, TIdentity> repository)
        where TEntity : class
        => repository.FindAllAsync(CancellationToken.None);

    public static Task<IPagedResult<TEntity>> FindAllPagedAsync<TEntity, TIdentity>(this IRepository<TEntity, TIdentity> repository, int offset, int pageSize)
        where TEntity : class
        => repository.FindAllPagedAsync(offset, pageSize, CancellationToken.None);
}
