namespace CrossCutting.Data.Sql;

public class DatabaseEntityRetriever<T>(
    DbConnection connection,
    IDatabaseEntityMapper<T> mapper) : IDatabaseEntityRetriever<T>
    where T : class
{
    private readonly DbConnection _connection = connection;
    private readonly IDatabaseEntityMapper<T> _mapper = mapper;

    private async Task<Result<TResult>> FindAsync<TResult>(Func<DbCommand, Task<TResult>> findDelegate)
    {
        var returnValue = default(TResult);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        {
            returnValue = await findDelegate(cmd).ConfigureAwait(false);
        }

        return returnValue is null
            ? Result.NotFound<TResult>()
            : Result.Success(returnValue);
    }

    public async Task<Result<T>> FindOneAsync(IDatabaseCommand command, CancellationToken token)
        => await FindAsync(async cmd => (await cmd.FindOneAsync(command.CommandText, command.CommandType, token, _mapper.Map, command.CommandParameters).ConfigureAwait(false))!).ConfigureAwait(false);

    public async Task<Result<IReadOnlyCollection<T>>> FindManyAsync(IDatabaseCommand command, CancellationToken token)
        => (await FindAsync(async cmd => (await cmd.FindManyAsync(command.CommandText, command.CommandType, token, _mapper.Map, command.CommandParameters).ConfigureAwait(false)).ToList()).ConfigureAwait(false)).TryCastAllowNull<IReadOnlyCollection<T>>();

    public async Task<Result<IPagedResult<T>>> FindPagedAsync(IPagedDatabaseCommand command, CancellationToken token)
    {
        var returnValue = default(IPagedResult<T>);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        using (var countCommand = _connection.CreateCommand())
        {
            countCommand.FillCommand(command.RecordCountCommand.CommandText, command.RecordCountCommand.CommandType, command.RecordCountCommand.CommandParameters);
            var totalRecordCount = ((await countCommand.ExecuteScalarAsync(token).ConfigureAwait(false)) as int?).GetValueOrDefault();
            returnValue = new PagedResult<T>
            (
                [.. await cmd.FindManyAsync(command.DataCommand.CommandText, command.DataCommand.CommandType, token, _mapper.Map, command.DataCommand.CommandParameters).ConfigureAwait(false)],
                totalRecordCount,
                command.Offset,
                command.PageSize
            );
        }

        return Result.Success(returnValue);
    }
}
