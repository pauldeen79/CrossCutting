namespace CrossCutting.Data.Sql;

public class DatabaseEntityRetriever<T> : IDatabaseEntityRetriever<T>
    where T : class
{
    private readonly IDbConnection _connection;
    private readonly IDatabaseEntityMapper<T> _mapper;
    private readonly ISqlCommandWrapperFactory _sqlCommandWrapperFactory;

    public DatabaseEntityRetriever(
        IDbConnection connection,
        IDatabaseEntityMapper<T> mapper,
        ISqlCommandWrapperFactory sqlCommandWrapperFactory)
    {
        _connection = connection;
        _mapper = mapper;
        _sqlCommandWrapperFactory = sqlCommandWrapperFactory;
    }

    public T? FindOne(IDatabaseCommand command)
        => Find(cmd => cmd.FindOne(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters));

    public IReadOnlyCollection<T> FindMany(IDatabaseCommand command)
        => Find(cmd => cmd.FindMany(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters).ToList());

    public IPagedResult<T> FindPaged(IPagedDatabaseCommand command)
    {
        var returnValue = default(IPagedResult<T>);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        {
            using (var countCommand = _connection.CreateCommand())
            {
                countCommand.FillCommand(command.RecordCountCommand.CommandText, command.RecordCountCommand.CommandType, command.RecordCountCommand.CommandParameters);
                var totalRecordCount = (int)countCommand.ExecuteScalar();
                returnValue = new PagedResult<T>
                (
                    cmd.FindMany(command.DataCommand.CommandText, command.DataCommand.CommandType, _mapper.Map, command.DataCommand.CommandParameters).ToList(),
                    totalRecordCount,
                    command.Offset,
                    command.PageSize
                );
            }
        }

        return returnValue;
    }

    private TResult Find<TResult>(Func<IDbCommand, TResult> findDelegate)
    {
        var returnValue = default(TResult);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        {
            returnValue = findDelegate(cmd);
        }

        return returnValue;
    }

    private async Task<TResult> FindAsync<TResult>(IDbConnection connection, Func<SqlCommandWrapper, Task<TResult>> findDelegate)
    {
        var returnValue = default(TResult);

        connection.OpenIfNecessary();
        using (var cmd = _sqlCommandWrapperFactory.Create(connection.CreateCommand()))
        {
            returnValue = await findDelegate(cmd);
        }

        return returnValue;
    }

    public async Task<T?> FindOneAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => await FindAsync(_connection, async cmd => await cmd.FindOneAsync(command.CommandText, command.CommandType, cancellationToken, _mapper.Map, command.CommandParameters));

    public async Task<IReadOnlyCollection<T>> FindManyAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => await FindAsync(_connection, async cmd => (await cmd.FindManyAsync(command.CommandText, command.CommandType, cancellationToken, _mapper.Map, command.CommandParameters)).ToList());

    public async Task<IPagedResult<T>> FindPagedAsync(IPagedDatabaseCommand command, CancellationToken cancellationToken)
    {
        var returnValue = default(IPagedResult<T>);

        _connection.OpenIfNecessary();
        using (var cmd = _sqlCommandWrapperFactory.Create(_connection.CreateCommand()))
        {
            using (var countCommand = _sqlCommandWrapperFactory.Create(_connection.CreateCommand()))
            {
                countCommand.FillCommand(command.RecordCountCommand.CommandText, command.RecordCountCommand.CommandType, command.RecordCountCommand.CommandParameters);
                var totalRecordCount = ((await countCommand.ExecuteScalarAsync(cancellationToken)) as int?).GetValueOrDefault();
                returnValue = new PagedResult<T>
                (
                    (await cmd.FindManyAsync(command.DataCommand.CommandText, command.DataCommand.CommandType, cancellationToken, _mapper.Map, command.DataCommand.CommandParameters)).ToList(),
                    totalRecordCount,
                    command.Offset,
                    command.PageSize
                );
            }
        }

        return returnValue;
    }
}
