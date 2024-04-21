namespace CrossCutting.Data.Sql;

public sealed class SqlCommandWrapper : IDbCommand
{
    private readonly IDbCommand _command;

    public SqlCommandWrapper(IDbCommand command)
    {
        _command = command;
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
}
