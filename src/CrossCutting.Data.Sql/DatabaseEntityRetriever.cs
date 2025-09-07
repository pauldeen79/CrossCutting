namespace CrossCutting.Data.Sql;

public class DatabaseEntityRetriever<T>(
    DbConnection connection,
    IDatabaseEntityMapper<T> mapper) : IDatabaseEntityRetriever<T>
    where T : class
{
    private readonly DbConnection _connection = connection;
    private readonly IDatabaseEntityMapper<T> _mapper = mapper;

    public Result<T> FindOne(IDatabaseCommand command)
        => Find(cmd => cmd.FindOne(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters)!);

    public Result<IReadOnlyCollection<T>> FindMany(IDatabaseCommand command)
        => Find(cmd => cmd.FindMany(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters).ToList()).TryCastAllowNull<IReadOnlyCollection<T>>();

    public Result<IPagedResult<T>> FindPaged(IPagedDatabaseCommand command)
    {
        var returnValue = default(IPagedResult<T>);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        using (var countCommand = _connection.CreateCommand())
        {
            countCommand.FillCommand(command.RecordCountCommand.CommandText, command.RecordCountCommand.CommandType, command.RecordCountCommand.CommandParameters);
            var totalRecordCount = (int)countCommand.ExecuteScalar();
            returnValue = new PagedResult<T>
            (
                [.. cmd.FindMany(command.DataCommand.CommandText, command.DataCommand.CommandType, _mapper.Map, command.DataCommand.CommandParameters)],
                totalRecordCount,
                command.Offset,
                command.PageSize
            );
        }

        return Result.Success(returnValue);
    }

    private Result<TResult> Find<TResult>(Func<IDbCommand, TResult> findDelegate)
    {
        var returnValue = default(TResult);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        {
            returnValue = findDelegate(cmd);
        }

        return returnValue is null
            ? Result.NotFound<TResult>()
            : Result.Success(returnValue);
    }

    private async Task<Result<TResult>> FindAsync<TResult>(Func<DbCommand, Task<TResult>> findDelegate)
    {
        var returnValue = default(TResult);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        {
            returnValue = await findDelegate(cmd);
        }

        return returnValue is null
            ? Result.NotFound<TResult>()
            : Result.Success(returnValue);
    }

    public async Task<Result<T>> FindOneAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => await FindAsync(async cmd => (await cmd.FindOneAsync(command.CommandText, command.CommandType, cancellationToken, _mapper.Map, command.CommandParameters))!);

    public async Task<Result<IReadOnlyCollection<T>>> FindManyAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => (await FindAsync(async cmd => (await cmd.FindManyAsync(command.CommandText, command.CommandType, cancellationToken, _mapper.Map, command.CommandParameters)).ToList())).TryCastAllowNull<IReadOnlyCollection<T>>();

    public async Task<Result<IPagedResult<T>>> FindPagedAsync(IPagedDatabaseCommand command, CancellationToken cancellationToken)
    {
        var returnValue = default(IPagedResult<T>);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        using (var countCommand = _connection.CreateCommand())
        {
            countCommand.FillCommand(command.RecordCountCommand.CommandText, command.RecordCountCommand.CommandType, command.RecordCountCommand.CommandParameters);
            var totalRecordCount = ((await countCommand.ExecuteScalarAsync(cancellationToken)) as int?).GetValueOrDefault();
            returnValue = new PagedResult<T>
            (
                [.. (await cmd.FindManyAsync(command.DataCommand.CommandText, command.DataCommand.CommandType, cancellationToken, _mapper.Map, command.DataCommand.CommandParameters))],
                totalRecordCount,
                command.Offset,
                command.PageSize
            );
        }

        return Result.Success(returnValue);
    }
}
