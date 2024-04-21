namespace CrossCutting.Data.Sql;

public sealed class SqlCommandWrapper : IDbCommand
{
    private readonly IDbCommand _command;
    private readonly Func<IDbCommand, CancellationToken, Task<int>> _executeNonQueryAsyncDelegate;
    private readonly Func<IDbCommand, CommandBehavior, CancellationToken, Task<SqlDataReaderWrapper>> _executeReaderAsyncDelegate;
    private readonly Func<IDbCommand, CancellationToken, Task<object?>> _executeScalarAsyncDelegate;

    public SqlCommandWrapper(
        IDbCommand command,
        Func<IDbCommand, CancellationToken, Task<int>> executeNonQueryAsyncDelegate,
        Func<IDbCommand, CommandBehavior, CancellationToken, Task<SqlDataReaderWrapper>> executeReaderAsyncDelegate,
        Func<IDbCommand, CancellationToken, Task<object?>> executeScalarAsyncDelegate)
    {
        _command = command;
        _executeNonQueryAsyncDelegate = executeNonQueryAsyncDelegate;
        _executeReaderAsyncDelegate = executeReaderAsyncDelegate;
        _executeScalarAsyncDelegate = executeScalarAsyncDelegate;
    }

    public string CommandText
    {
        get
        {
            return _command.CommandText;
        }
        set
        {
            _command.CommandText = value;
        }
    }

    public int CommandTimeout
    {
        get
        {
            return _command.CommandTimeout;
        }
        set
        {
            _command.CommandTimeout = value;
        }
    }

    public CommandType CommandType
    {
        get
        {
            return _command.CommandType;
        }
        set
        {
            _command.CommandType = value;
        }
    }

    public IDbConnection Connection
    {
        get
        {
            return _command.Connection;
        }
        set
        {
            _command.Connection = value;
        }
    }

    public IDataParameterCollection Parameters
    {
        get
        {
            return _command.Parameters;
        }
    }

    public IDbTransaction Transaction
    {
        get
        {
            return _command.Transaction;
        }
        set
        {
            _command.Transaction = value;
        }
    }

    public UpdateRowSource UpdatedRowSource
    {
        get
        {
            return _command.UpdatedRowSource;
        }
        set
        {
            _command.UpdatedRowSource = value;
        }
    }

    public void Cancel() => _command.Cancel();

    public IDbDataParameter CreateParameter() => _command.CreateParameter();

    public void Dispose() => _command.Dispose();

    public int ExecuteNonQuery() => _command.ExecuteNonQuery();

    public IDataReader ExecuteReader() => _command.ExecuteReader();

    public IDataReader ExecuteReader(CommandBehavior behavior) => _command.ExecuteReader(behavior);

    public object ExecuteScalar() => _command.ExecuteScalar();

    public void Prepare() => _command.Prepare();

    public Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken) => _executeNonQueryAsyncDelegate(_command, cancellationToken);

    public Task<SqlDataReaderWrapper> ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken) => _executeReaderAsyncDelegate(_command, behavior, cancellationToken);

    public Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken) => _executeScalarAsyncDelegate(_command, cancellationToken);
}
