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
    public async Task<Result<TEntity>> AddAsync(TEntity instance, CancellationToken cancellationToken)
        => await EntityCommandProvider.CreateAsync(instance, DatabaseOperation.Insert).Result
            .EnsureNotNull()
            .EnsureValue()
            .OnSuccessAsync(async databaseCommand => (await CommandProcessor.ExecuteCommandAsync(databaseCommand, instance, cancellationToken).ConfigureAwait(false))
                .HandleResult($"{typeof(TEntity).Name} has not been added"))
            .ConfigureAwait(false);

    public async Task<Result<TEntity>> UpdateAsync(TEntity instance, CancellationToken cancellationToken)
        => await EntityCommandProvider.CreateAsync(instance, DatabaseOperation.Update).Result
            .EnsureNotNull()
            .EnsureValue()
            .OnSuccessAsync(async databaseCommand => (await CommandProcessor.ExecuteCommandAsync(databaseCommand, instance, cancellationToken).ConfigureAwait(false))
                .HandleResult($"{typeof(TEntity).Name} has not been updated"))
            .ConfigureAwait(false);

    public async Task<Result<TEntity>> DeleteAsync(TEntity instance, CancellationToken cancellationToken)
        => await EntityCommandProvider.CreateAsync(instance, DatabaseOperation.Delete).Result
            .EnsureNotNull()
            .EnsureValue()
            .OnSuccessAsync(async databaseCommand => (await CommandProcessor.ExecuteCommandAsync(databaseCommand, instance, cancellationToken).ConfigureAwait(false))
                .HandleResult($"{typeof(TEntity).Name} has not been deleted"))
            .ConfigureAwait(false);

    public async Task<Result<TEntity>> FindAsync(TIdentity identity, CancellationToken cancellationToken)
        => await IdentitySelectCommandProvider.CreateAsync(identity, DatabaseOperation.Select).Result
            .EnsureNotNull()
            .EnsureValue()
            .OnSuccessAsync(databaseCommand => EntityRetriever.FindOneAsync(databaseCommand, cancellationToken))
            .ConfigureAwait(false);

    public async Task<Result<IReadOnlyCollection<TEntity>>> FindAllAsync(CancellationToken cancellationToken)
        => await EntitySelectCommandProvider.CreateAsync<TEntity>(DatabaseOperation.Select).Result
            .EnsureNotNull()
            .EnsureValue()
            .OnSuccessAsync(databaseCommand => EntityRetriever.FindManyAsync(databaseCommand, cancellationToken))
            .ConfigureAwait(false);

    public async Task<Result<IPagedResult<TEntity>>> FindAllPagedAsync(int offset, int pageSize, CancellationToken cancellationToken)
        => await (await PagedEntitySelectCommandProvider.CreatePagedAsync<TEntity>(DatabaseOperation.Select, offset, pageSize).ConfigureAwait(false))
            .EnsureNotNull()
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
