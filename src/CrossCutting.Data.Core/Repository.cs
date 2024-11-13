namespace CrossCutting.Data.Core;

public class Repository<TEntity, TIdentity>(IDatabaseCommandProcessor<TEntity> commandProcessor,
                  IDatabaseEntityRetriever<TEntity> entityRetriever,
                  IDatabaseCommandProvider<TIdentity> identitySelectCommandProvider,
                  IPagedDatabaseCommandProvider pagedEntitySelectCommandProvider,
                  IDatabaseCommandProvider entitySelectCommandProvider,
                  IDatabaseCommandProvider<TEntity> entityCommandProvider) : IRepository<TEntity, TIdentity>
    where TEntity : class
{
    public TEntity Add(TEntity instance)
        => CommandProcessor.ExecuteCommand(EntityCommandProvider.Create(instance, DatabaseOperation.Insert), instance)
                           .HandleResult($"{typeof(TEntity).Name} has not been added");

    public TEntity Update(TEntity instance)
        => CommandProcessor.ExecuteCommand(EntityCommandProvider.Create(instance, DatabaseOperation.Update), instance)
                           .HandleResult($"{typeof(TEntity).Name} has not been updated");

    public TEntity Delete(TEntity instance)
        => CommandProcessor.ExecuteCommand(EntityCommandProvider.Create(instance, DatabaseOperation.Delete), instance)
                           .HandleResult($"{typeof(TEntity).Name} has not been deleted");

    public TEntity? Find(TIdentity identity)
        => EntityRetriever.FindOne(IdentitySelectCommandProvider.Create(identity, DatabaseOperation.Select));

    public IReadOnlyCollection<TEntity> FindAll()
        => EntityRetriever.FindMany(EntitySelectCommandProvider.Create<TEntity>(DatabaseOperation.Select));

    public IPagedResult<TEntity> FindAllPaged(int offset, int pageSize)
        => EntityRetriever.FindPaged(PagedEntitySelectCommandProvider.CreatePaged<TEntity>(DatabaseOperation.Select, offset, pageSize));

    public async Task<TEntity> AddAsync(TEntity instance, CancellationToken cancellationToken)
        => (await CommandProcessor.ExecuteCommandAsync(EntityCommandProvider.Create(instance, DatabaseOperation.Insert), instance, cancellationToken).ConfigureAwait(false))
                                  .HandleResult($"{typeof(TEntity).Name} has not been added");

    public async Task<TEntity> UpdateAsync(TEntity instance, CancellationToken cancellationToken)
        => (await CommandProcessor.ExecuteCommandAsync(EntityCommandProvider.Create(instance, DatabaseOperation.Update), instance, cancellationToken).ConfigureAwait(false))
                                  .HandleResult($"{typeof(TEntity).Name} has not been updated");

    public async Task<TEntity> DeleteAsync(TEntity instance, CancellationToken cancellationToken)
        => (await CommandProcessor.ExecuteCommandAsync(EntityCommandProvider.Create(instance, DatabaseOperation.Delete), instance, cancellationToken).ConfigureAwait(false))
                                  .HandleResult($"{typeof(TEntity).Name} has not been deleted");

    public async Task<TEntity?> FindAsync(TIdentity identity, CancellationToken cancellationToken)
        => await EntityRetriever.FindOneAsync(IdentitySelectCommandProvider.Create(identity, DatabaseOperation.Select), cancellationToken)
                                .ConfigureAwait(false);

    public async Task<IReadOnlyCollection<TEntity>> FindAllAsync(CancellationToken cancellationToken)
        => await EntityRetriever.FindManyAsync(EntitySelectCommandProvider.Create<TEntity>(DatabaseOperation.Select), cancellationToken)
                                .ConfigureAwait(false);

    public async Task<IPagedResult<TEntity>> FindAllPagedAsync(int offset, int pageSize, CancellationToken cancellationToken)
        => await EntityRetriever.FindPagedAsync(PagedEntitySelectCommandProvider.CreatePaged<TEntity>(DatabaseOperation.Select, offset, pageSize), cancellationToken)
                                .ConfigureAwait(false);

    protected IDatabaseCommandProcessor<TEntity> CommandProcessor { get; } = commandProcessor;
    protected IDatabaseEntityRetriever<TEntity> EntityRetriever { get; } = entityRetriever;
    protected IDatabaseCommandProvider<TIdentity> IdentitySelectCommandProvider { get; } = identitySelectCommandProvider;
    protected IDatabaseCommandProvider EntitySelectCommandProvider { get; } = entitySelectCommandProvider;
    protected IPagedDatabaseCommandProvider PagedEntitySelectCommandProvider { get; } = pagedEntitySelectCommandProvider;
    protected IDatabaseCommandProvider<TEntity> EntityCommandProvider { get; } = entityCommandProvider;
}
