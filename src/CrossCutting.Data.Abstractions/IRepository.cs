namespace CrossCutting.Data.Abstractions;

public interface IRepository<TEntity, in TIdentity>
    where TEntity : class
{
    TEntity Add(TEntity instance);
    TEntity Update(TEntity instance);
    TEntity Delete(TEntity instance);
    TEntity? Find(TIdentity identity);
    IReadOnlyCollection<TEntity> FindAll();
    IPagedResult<TEntity> FindAllPaged(int offset, int pageSize);
}
