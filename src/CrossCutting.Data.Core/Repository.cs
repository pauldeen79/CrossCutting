namespace CrossCutting.Data.Core;

public class Repository<TEntity, TIdentity>(IDatabaseCommandProcessor<TEntity> commandProcessor,
                  IDatabaseEntityRetriever<TEntity> entityRetriever,
                  IDatabaseCommandProvider<TIdentity> identitySelectCommandProvider,
                  IPagedDatabaseCommandProvider pagedEntitySelectCommandProvider,
                  IDatabaseCommandProvider entitySelectCommandProvider,
                  IDatabaseCommandProvider<TEntity> entityCommandProvider) : IRepository<TEntity, TIdentity>
    where TEntity : class
{
    public Result<TEntity> Add(TEntity instance)
    {
        var commandResult = EntityCommandProvider.Create(instance, DatabaseOperation.Insert).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<TEntity>(commandResult);
        }

        return CommandProcessor.ExecuteCommand(commandResult.Value!, instance)
                               .HandleResult($"{typeof(TEntity).Name} has not been added");
    }

    public Result<TEntity> Update(TEntity instance)
    {
        var commandResult = EntityCommandProvider.Create(instance, DatabaseOperation.Update).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<TEntity>(commandResult);
        }

        return CommandProcessor.ExecuteCommand(commandResult.Value!, instance)
                               .HandleResult($"{typeof(TEntity).Name} has not been updated");
    }

    public Result<TEntity> Delete(TEntity instance)
    {
        var commandResult = EntityCommandProvider.Create(instance, DatabaseOperation.Delete).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<TEntity>(commandResult);
        }

        return CommandProcessor.ExecuteCommand(commandResult.Value!, instance)
                               .HandleResult($"{typeof(TEntity).Name} has not been deleted");
    }

    public Result<TEntity> Find(TIdentity identity)
    {
        var commandResult = IdentitySelectCommandProvider.Create(identity, DatabaseOperation.Select).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<TEntity>(commandResult);
        }

        return EntityRetriever.FindOne(commandResult.Value!);
    }

    public Result<IReadOnlyCollection<TEntity>> FindAll()
    {
        var commandResult = EntitySelectCommandProvider.Create<TEntity>(DatabaseOperation.Select).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<IReadOnlyCollection<TEntity>>(commandResult);
        }

        return EntityRetriever.FindMany(commandResult.Value!);
    }

    public Result<IPagedResult<TEntity>> FindAllPaged(int offset, int pageSize)
    {
        var commandResult = PagedEntitySelectCommandProvider.CreatePaged<TEntity>(DatabaseOperation.Select, offset, pageSize).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<IPagedResult<TEntity>>(commandResult);
        }

        return EntityRetriever.FindPaged(commandResult.Value!);
    }

    public async Task<Result<TEntity>> AddAsync(TEntity instance, CancellationToken cancellationToken)
    {
        var commandResult = EntityCommandProvider.Create(instance, DatabaseOperation.Insert).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<TEntity>(commandResult);
        }

        return (await CommandProcessor.ExecuteCommandAsync(commandResult.Value!, instance, cancellationToken).ConfigureAwait(false))
                                      .HandleResult($"{typeof(TEntity).Name} has not been added");
    }

    public async Task<Result<TEntity>> UpdateAsync(TEntity instance, CancellationToken cancellationToken)
    {
        var commandResult = EntityCommandProvider.Create(instance, DatabaseOperation.Update).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<TEntity>(commandResult);
        }

        return (await CommandProcessor.ExecuteCommandAsync(commandResult.Value!, instance, cancellationToken).ConfigureAwait(false))
                                      .HandleResult($"{typeof(TEntity).Name} has not been updated");
    }

    public async Task<Result<TEntity>> DeleteAsync(TEntity instance, CancellationToken cancellationToken)
    {
        var commandResult = EntityCommandProvider.Create(instance, DatabaseOperation.Delete).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<TEntity>(commandResult);
        }

        return (await CommandProcessor.ExecuteCommandAsync(commandResult.Value!, instance, cancellationToken).ConfigureAwait(false))
                                      .HandleResult($"{typeof(TEntity).Name} has not been deleted");
    }

    public async Task<Result<TEntity>> FindAsync(TIdentity identity, CancellationToken cancellationToken)
    {
        var commandResult = IdentitySelectCommandProvider.Create(identity, DatabaseOperation.Select).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<TEntity>(commandResult);
        }

        return await EntityRetriever.FindOneAsync(commandResult.Value!, cancellationToken)
                                    .ConfigureAwait(false);
    }

    public async Task<Result<IReadOnlyCollection<TEntity>>> FindAllAsync(CancellationToken cancellationToken)
    {
        var commandResult = EntitySelectCommandProvider.Create<TEntity>(DatabaseOperation.Select).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<IReadOnlyCollection<TEntity>>(commandResult);
        }

        return await EntityRetriever.FindManyAsync(commandResult.Value!, cancellationToken)
                                    .ConfigureAwait(false);
    }

    public async Task<Result<IPagedResult<TEntity>>> FindAllPagedAsync(int offset, int pageSize, CancellationToken cancellationToken)
    {
        var commandResult = PagedEntitySelectCommandProvider.CreatePaged<TEntity>(DatabaseOperation.Select, offset, pageSize).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<IPagedResult<TEntity>>(commandResult);
        }

        return await EntityRetriever.FindPagedAsync(commandResult.Value!, cancellationToken)
                                    .ConfigureAwait(false);
    }

    protected IDatabaseCommandProcessor<TEntity> CommandProcessor { get; } = commandProcessor;
    protected IDatabaseEntityRetriever<TEntity> EntityRetriever { get; } = entityRetriever;
    protected IDatabaseCommandProvider<TIdentity> IdentitySelectCommandProvider { get; } = identitySelectCommandProvider;
    protected IDatabaseCommandProvider EntitySelectCommandProvider { get; } = entitySelectCommandProvider;
    protected IPagedDatabaseCommandProvider PagedEntitySelectCommandProvider { get; } = pagedEntitySelectCommandProvider;
    protected IDatabaseCommandProvider<TEntity> EntityCommandProvider { get; } = entityCommandProvider;
}
