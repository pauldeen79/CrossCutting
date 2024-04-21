namespace CrossCutting.Data.Sql;

public class DatabaseEntityRetriever<T> : IDatabaseEntityRetriever<T>
    where T : class
{
    private readonly IDbConnection _connection;
    private readonly IDatabaseEntityMapper<T> _mapper;

    public DatabaseEntityRetriever(IDbConnection connection, IDatabaseEntityMapper<T> mapper)
    {
        _connection = connection;
        _mapper = mapper;
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

    private static async Task<TResult> FindAsync<TResult>(SqlConnection connection, Func<SqlCommand, Task<TResult>> findDelegate)
    {
        var returnValue = default(TResult);

        connection.OpenIfNecessary();
        using (var cmd = connection.CreateCommand())
        {
            returnValue = await findDelegate(cmd);
        }

        return returnValue;
    }

    public async Task<T?> FindOneAsync(IDatabaseCommand command, CancellationToken cancellationToken)
    {
        if (_connection is not SqlConnection sqlConnection)
        {
            return FindOne(command);
        }

        return await FindAsync(sqlConnection, async cmd => await cmd.FindOneAsync(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters));
    }

    public async Task<IReadOnlyCollection<T>> FindManyAsync(IDatabaseCommand command, CancellationToken cancellationToken)
    {
        if (_connection is not SqlConnection sqlConnection)
        {
            return FindMany(command);
        }

        return await FindAsync(sqlConnection, async cmd => (await cmd.FindManyAsync(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters)).ToList());
    }

    public async Task<IPagedResult<T>> FindPagedAsync(IPagedDatabaseCommand command, CancellationToken cancellationToken)
    {
        if (_connection is not SqlConnection sqlConnection)
        {
            return FindPaged(command);
        }

        var returnValue = default(IPagedResult<T>);

        sqlConnection.OpenIfNecessary();
        using (var cmd = sqlConnection.CreateCommand())
        {
            using (var countCommand = sqlConnection.CreateCommand())
            {
                countCommand.FillCommand(command.RecordCountCommand.CommandText, command.RecordCountCommand.CommandType, command.RecordCountCommand.CommandParameters);
                var totalRecordCount = (int) await countCommand.ExecuteScalarAsync();
                returnValue = new PagedResult<T>
                (
                    (await cmd.FindManyAsync(command.DataCommand.CommandText, command.DataCommand.CommandType, _mapper.Map, command.DataCommand.CommandParameters)).ToList(),
                    totalRecordCount,
                    command.Offset,
                    command.PageSize
                );
            }
        }

        return returnValue;
    }
}
