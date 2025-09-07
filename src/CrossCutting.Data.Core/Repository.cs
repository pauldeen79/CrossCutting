namespace CrossCutting.Data.Core;

public class Repository<TEntity, TIdentity>(
    IDatabaseCommandProcessor<TEntity> commandProcessor,
    IDatabaseEntityRetriever<TEntity> entityRetriever,
    IDatabaseCommandProvider<TIdentity> identitySelectCommandProvider,
    IPagedDatabaseCommandProvider pagedEntitySelectCommandProvider,
    IDatabaseCommandProvider entitySelectCommandProvider,
    IDatabaseCommandProvider<TEntity> entityCommandProvider) : IRepository<TEntity, TIdentity>
    where TEntity : class
{
    public Result<TEntity> Add(TEntity instance)
        => EntityCommandProvider.Create(instance, DatabaseOperation.Insert)
            .EnsureValue()
            .OnSuccess(databaseCommand => CommandProcessor.ExecuteCommand(databaseCommand, instance)
                .HandleResult($"{typeof(TEntity).Name} has not been added"));

    public Result<TEntity> Update(TEntity instance)
        => EntityCommandProvider.Create(instance, DatabaseOperation.Update)
            .EnsureValue()
            .OnSuccess(databaseCommand => CommandProcessor.ExecuteCommand(databaseCommand, instance)
                .HandleResult($"{typeof(TEntity).Name} has not been updated"));

    public Result<TEntity> Delete(TEntity instance)
        => EntityCommandProvider.Create(instance, DatabaseOperation.Delete)
            .EnsureValue()
            .OnSuccess(databaseCommand => CommandProcessor.ExecuteCommand(databaseCommand, instance)
                .HandleResult($"{typeof(TEntity).Name} has not been deleted"));

    public Result<TEntity> Find(TIdentity identity)
        => IdentitySelectCommandProvider.Create(identity, DatabaseOperation.Select)
            .EnsureValue()
            .OnSuccess(EntityRetriever.FindOne);

    public Result<IReadOnlyCollection<TEntity>> FindAll()
        => EntitySelectCommandProvider.Create<TEntity>(DatabaseOperation.Select)
            .EnsureValue()
            .OnSuccess(EntityRetriever.FindMany);

    public Result<IPagedResult<TEntity>> FindAllPaged(int offset, int pageSize)
        => PagedEntitySelectCommandProvider.CreatePaged<TEntity>(DatabaseOperation.Select, offset, pageSize)
            .EnsureValue()
            .OnSuccess(EntityRetriever.FindPaged);

    public async Task<Result<TEntity>> AddAsync(TEntity instance, CancellationToken cancellationToken)
        => await EntityCommandProvider.Create(instance, DatabaseOperation.Insert)
            .EnsureValue()
            .OnSuccessAsync(async databaseCommand => (await CommandProcessor.ExecuteCommandAsync(databaseCommand, instance, cancellationToken).ConfigureAwait(false))
                .HandleResult($"{typeof(TEntity).Name} has not been added"))
            .ConfigureAwait(false);

    public async Task<Result<TEntity>> UpdateAsync(TEntity instance, CancellationToken cancellationToken)
        => await EntityCommandProvider.Create(instance, DatabaseOperation.Update)
            .EnsureValue()
            .OnSuccessAsync(async databaseCommand => (await CommandProcessor.ExecuteCommandAsync(databaseCommand, instance, cancellationToken).ConfigureAwait(false))
                .HandleResult($"{typeof(TEntity).Name} has not been updated"))
            .ConfigureAwait(false);

    public async Task<Result<TEntity>> DeleteAsync(TEntity instance, CancellationToken cancellationToken)
        => await EntityCommandProvider.Create(instance, DatabaseOperation.Delete)
            .EnsureValue()
            .OnSuccessAsync(async databaseCommand => (await CommandProcessor.ExecuteCommandAsync(databaseCommand, instance, cancellationToken).ConfigureAwait(false))
                .HandleResult($"{typeof(TEntity).Name} has not been deleted"))
            .ConfigureAwait(false);

    public async Task<Result<TEntity>> FindAsync(TIdentity identity, CancellationToken cancellationToken)
        => await IdentitySelectCommandProvider.Create(identity, DatabaseOperation.Select)
            .EnsureValue()
            .OnSuccessAsync(databaseCommand => EntityRetriever.FindOneAsync(databaseCommand, cancellationToken))
            .ConfigureAwait(false);

    public async Task<Result<IReadOnlyCollection<TEntity>>> FindAllAsync(CancellationToken cancellationToken)
        => await EntitySelectCommandProvider.Create<TEntity>(DatabaseOperation.Select)
            .EnsureValue()
            .OnSuccessAsync(databaseCommand => EntityRetriever.FindManyAsync(databaseCommand, cancellationToken))
            .ConfigureAwait(false);

    public async Task<Result<IPagedResult<TEntity>>> FindAllPagedAsync(int offset, int pageSize, CancellationToken cancellationToken)
        => await PagedEntitySelectCommandProvider.CreatePaged<TEntity>(DatabaseOperation.Select, offset, pageSize)
            .EnsureValue()
            .OnSuccessAsync(databaseCommand => EntityRetriever.FindPagedAsync(databaseCommand, cancellationToken))
            .ConfigureAwait(false);

    protected IDatabaseCommandProcessor<TEntity> CommandProcessor { get; } = commandProcessor;
    protected IDatabaseEntityRetriever<TEntity> EntityRetriever { get; } = entityRetriever;
    protected IDatabaseCommandProvider<TIdentity> IdentitySelectCommandProvider { get; } = identitySelectCommandProvider;
    protected IDatabaseCommandProvider EntitySelectCommandProvider { get; } = entitySelectCommandProvider;
    protected IPagedDatabaseCommandProvider PagedEntitySelectCommandProvider { get; } = pagedEntitySelectCommandProvider;
    protected IDatabaseCommandProvider<TEntity> EntityCommandProvider { get; } = entityCommandProvider;
}
